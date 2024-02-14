using Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Interfaces;
using Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Enums;
using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Services
{
    public class ConnectedPanelService : IConnectedPanelService, IDisposable
    {
        private readonly IOverlayService _overlayService;

        private bool _isOpen = false;

        private OverlayReference _menuReference;


        public ConnectedPanelService(IOverlayService overlayService)
        {
            _overlayService = overlayService;
        }

        public OverlayReference ToggleMenu(Type componentType, IDictionary<string, object> parameters, ElementReference menuTrigger, ConnectedPanelConfig connectedPanelConfig = null)
        {
            if (!_isOpen)
            {
                var reference = Open(componentType, parameters, menuTrigger, connectedPanelConfig);

                _isOpen = !_isOpen;

                return reference;
            }
            else
            {
                Close();
            }

            _isOpen = !_isOpen;

            return null;
        }

        public OverlayReference Open(Type componentType, IDictionary<string, object> parameters, ElementReference menuTrigger, ConnectedPanelConfig connectedPanelConfig = null)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType))
            {
                throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
            }

            var positions = new List<ConnectedPosition>
            {
                new()
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Top
                },
                new()
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Bottom
                },
                new()
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.End,
                    OverlayY = VerticalConnectionPos.Top
                },
                new()
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.End,
                    OverlayY = VerticalConnectionPos.Bottom
                }
            };

            var flexibleConnectedPositionStrategyBuilder = new FlexibleConnectedPositionStrategyBuilder().WithLockedPosition()
            .WithViewportMargin(8)
            .SetOrigin(menuTrigger)
            .WithFlexibleDimensions(false)
            .WithPositions(positions);

            ConnectedPanelConfig config = new();

            if (connectedPanelConfig != null)
            {
                config = connectedPanelConfig;
            }

            var overlayConfig = new OverlayConfig()
            {
                HasBackdrop = config.HasBackdrop,
                HasTransparentBackdrop = config.HasTransparentBackdrop,
                PositionStrategy = PositionStrategy.FlexibleConnectedTo,
                Width = config.Width,
                Height = config.Height,
                FlexibleConnectedPositionStrategyBuilder = flexibleConnectedPositionStrategyBuilder,
                ComponentType = ComponentType.ConnectedPanel
            };

            var connectedPanel = new RenderFragment(x =>
            {
                x.OpenComponent(0, typeof(DnetConnectedFloatingPanel));
                x.AddAttribute(1, "ComponentType", componentType);
                x.AddAttribute(2, "Parameters", parameters);
                x.AddAttribute(3, "ConnectedPanelClasses", config.ConnectedPanelClasses);
                x.CloseComponent();
            });

            _menuReference = _overlayService.Attach(connectedPanel, overlayConfig);

            _menuReference.Close += CloseDialog;

            return _menuReference;
        }

        public void Close()
        {
            _isOpen = false;

            var result = new OverlayResult
            {
                OverlayReferenceId = _menuReference.GetOverlayReferenceId(),
                CloseReason = CloseReason.Cancel
            };

            _overlayService.Detach(result);
        }

        public void Close(OverlayResult overlayDataResult)
        {
            _isOpen = false;

            _overlayService.Detach(overlayDataResult);
        }

        void CloseDialog(OverlayResult overlayDataResult)
        {
            _isOpen = false;
        }

        public void Dispose()
        {
            if (_menuReference != null) _menuReference.Close -= CloseDialog;
        }
    }
}
