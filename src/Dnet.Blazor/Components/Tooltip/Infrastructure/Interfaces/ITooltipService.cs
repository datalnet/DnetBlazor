using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Models;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Tooltip.Infrastructure.Interfaces
{
    public interface ITooltipService
    {
        OverlayReference Show(TooltipConfig tooltipConfig, ElementReference elementReference);

        OverlayReference Show<TComponent>(TooltipConfig tooltipConfig, ElementReference elementReference) where TComponent : ComponentBase;

        void Close(OverlayResult overlayDataResult);
    }
}
