using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Toast.Infrastructure.Enums;
using Dnet.Blazor.Components.Toast.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Toast.Infrastructure.Models;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Toast.Infrastructure.Services
{
    public class ToastService : IToastService
    {
        private readonly IOverlayService _overlayService;

        private int _toastCounter = 0;

        private Dictionary<int, int> _positionTracker = new Dictionary<int, int>();

        public ToastService(IOverlayService overlayService)
        {
            _overlayService = overlayService;
        }

        public void Show(ToastConfig toastConfig, Type componentType, IDictionary<string, object> parameters, RenderFragment dialogContent)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType) && componentType != null)
            {
                throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
            }

            var globalPositionStrategy = new GlobalPositionStrategyBuilder();

            var offsetBottom = toastConfig.OffsetBottom > 0 ? toastConfig.OffsetBottom : null;

            var offsetRight = toastConfig.OffsetRight > 0 ? toastConfig.OffsetRight : null;

            var offsetTop = toastConfig.OffsetTop > 0 ? toastConfig.OffsetTop : null;

            var offsetLeft = toastConfig.OffsetLeft > 0 ? toastConfig.OffsetLeft : null;

            var d = 0;
            var position = 0; //0, 4, 5
            foreach (var entry in _positionTracker.OrderBy(p => p.Key))
            {
                if (entry.Key == d)
                {
                    d++;
                }
                else
                {
                    position = d;
                    break;
                }

                position = d;
            }

            switch (toastConfig.ToastPostion)
            {
                case ToastPostion.BottomCenter:

                    globalPositionStrategy.Bottom($"{offsetBottom + ((toastConfig.Height + toastConfig.Margin) * position)}px");
                    globalPositionStrategy.CenterHorizontally("");

                    break;

                case ToastPostion.BottomRight:

                    globalPositionStrategy.Bottom($"{offsetBottom + ((toastConfig.Height + toastConfig.Margin) * position)}px");
                    globalPositionStrategy.Right(offsetRight + "px");

                    break;

                case ToastPostion.BottomLeft:

                    globalPositionStrategy.Bottom(offsetBottom + "px");
                    globalPositionStrategy.Left(offsetLeft + "px");

                    break;

                case ToastPostion.TopCenter:

                    globalPositionStrategy.Top(offsetTop + "px");
                    globalPositionStrategy.CenterHorizontally("");

                    break;

                case ToastPostion.TopRight:

                    globalPositionStrategy.Top($"{offsetTop + ((toastConfig.Height + toastConfig.Margin) * position)}px");
                    globalPositionStrategy.Right(offsetRight + "px");

                    break;

                case ToastPostion.TopLeft:

                    globalPositionStrategy.Top($"{offsetTop + ((toastConfig.Height + toastConfig.Margin) * position)}px");
                    globalPositionStrategy.Left(offsetLeft + "px");

                    break;
                case ToastPostion.LeftCenter:

                    globalPositionStrategy.Left(offsetLeft + "px");
                    globalPositionStrategy.CenterVertically("");

                    break;

                case ToastPostion.RightCenter:

                    globalPositionStrategy.Right(offsetRight + "px");
                    globalPositionStrategy.CenterVertically("");

                    break;
            }

            var overlayConfig = new OverlayConfig()
            {
                HasBackdrop = toastConfig.HasBackdrop,
                HasTransparentBackdrop = toastConfig.HasTransparentBackdrop,
                Width = toastConfig.Width + "px",
                Height = toastConfig.Height + "px",
                GlobalPositionStrategy = globalPositionStrategy,
                MaxHeight = "170px",
                ComponentType = ComponentType.Toast
            };

            var toast = new RenderFragment(x =>
            {
                x.OpenComponent(0, typeof(DnetToast));
                x.AddAttribute(1, "Title", toastConfig.Title);
                x.AddAttribute(2, "ToastClass", toastConfig.ToastClass);
                x.AddAttribute(3, "Text", toastConfig.Text);
                x.AddAttribute(4, "ToastType", toastConfig.ToastType);
                x.AddAttribute(5, "ToastTypeIconClass", toastConfig.ToastTypeIconClass);
                x.AddAttribute(6, "TypeIconClass", toastConfig.ToastTypeColor);
                x.AddAttribute(7, "ExcutionTime", toastConfig.ExcutionTime);
                x.AddAttribute(8, "ShowExcutionTime", toastConfig.ShowExcutionTime);
                if (componentType != null && parameters.Any()) x.AddAttribute(9, "Parameters", parameters);
                if (componentType != null) x.AddAttribute(10, "ComponentType", componentType);
                if (dialogContent != null) x.AddAttribute(12, "ContentChild", dialogContent);
                x.CloseComponent();
            });

            var reference = _overlayService.Attach(toast, overlayConfig);

            _toastCounter++;

            var c = 0;
            var key = 0; // 0, 3, 5
            foreach (var entry in _positionTracker.OrderBy(p => p.Key))
            {
                if (entry.Key == c)
                {
                    c++;
                }
                else
                {
                    key = c;
                    break;
                }

                key = c;
            }

            _positionTracker.Add(!_positionTracker.Any() ? 0 : key, reference.GetOverlayReferenceId());
        }

        public void Close(OverlayResult overlayDataResult)
        {
            if (_toastCounter > 0)
            {
                var item = _positionTracker.FirstOrDefault(p => p.Value == overlayDataResult.OverlayReferenceId);
                _positionTracker.Remove(item.Key);
            }

            _toastCounter--;

            if (_toastCounter < 0) { _toastCounter = 0; }

            _overlayService.Detach(overlayDataResult);
        }
    }
}
