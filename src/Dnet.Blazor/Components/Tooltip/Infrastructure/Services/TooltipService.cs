using Dnet.Blazor.Components.Overlay.Infrastructure.Enums;
using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Models;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Tooltip.Infrastructure.Services
{
    public class TooltipService : ITooltipService
    {
        private readonly IOverlayService _overlayService;

        public TooltipService(IOverlayService overlayService)
        {
            _overlayService = overlayService;
        }

        public OverlayReference Show(TooltipConfig tooltipConfig, ElementReference elementReference)
        {
            return Open(null, tooltipConfig, elementReference);
        }

        public OverlayReference Show<TComponent>(TooltipConfig tooltipConfig, ElementReference elementReference) where TComponent : ComponentBase
        {
            return Open(typeof(TComponent), tooltipConfig, elementReference);
        }

        private OverlayReference Open(Type componentType, TooltipConfig tooltipConfig, ElementReference elementReference)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType) && componentType != null)
            {
                throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
            }

            var positions = new List<ConnectedPosition>
            {
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Center,
                    OverlayX = HorizontalConnectionPos.End,
                    OverlayY = VerticalConnectionPos.Center
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Center,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Center
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Center,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.Center,
                    OverlayY = VerticalConnectionPos.Bottom
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Center,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.Center,
                    OverlayY = VerticalConnectionPos.Top
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Top
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Bottom
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Top
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Bottom
                }
            };

            var flexibleConnectedPositionStrategyBuilder = new FlexibleConnectedPositionStrategyBuilder()
                .WithViewportMargin(8)
                .WithFlexibleDimensions(false)
                .SetOrigin(elementReference)
                .WithPositions(positions);

            var overlayConfig = new OverlayConfig
            {
                HasBackdrop = false,
                HasTransparentBackdrop = true,
                PositionStrategy = PositionStrategy.FlexibleConnectedTo,
                FlexibleConnectedPositionStrategyBuilder = flexibleConnectedPositionStrategyBuilder,
                PanelClass = "dnet-tooltip-panel",
                Width = tooltipConfig.Width,
                Height = tooltipConfig.Height,
                MinHeight = tooltipConfig.MinHeight,
                MinWidth = tooltipConfig.MinWidth,
                MaxHeight = tooltipConfig.MaxHeight,
                MaxWidth = tooltipConfig.MaxWidth,
                ComponentType = ComponentType.ToolTip
            };

            var userContent = new RenderFragment(x => { });

            if (componentType != null)
            {
                userContent = x =>
                {
                    x.OpenComponent(0, componentType);
                    x.AddAttribute(1, "ContentData", tooltipConfig.Text);
                    x.CloseComponent();
                };
            }

            var tooltip = new RenderFragment(x =>
            {
                x.OpenComponent(0, typeof(DnetTooltipPanel));
                x.AddAttribute(1, "Text", tooltipConfig.Text);
                x.AddAttribute(2, "TooltipClass", tooltipConfig.TooltipClass);
                x.AddAttribute(3, "TooltipColor", tooltipConfig.TooltipColor);
                x.AddAttribute(4, "MaxWidth", tooltipConfig.MaxWidth);
                x.AddAttribute(5, "MaxHeight", tooltipConfig.MaxHeight);
                x.AddAttribute(6, "ContentChild", userContent);
                x.CloseComponent();
            });

            var overlayReference = _overlayService.Attach(tooltip, overlayConfig);

            return overlayReference;
        }

        public void Close(OverlayResult overlayDataResult)
        {
            _overlayService.Detach(overlayDataResult);
        }
    }
}
