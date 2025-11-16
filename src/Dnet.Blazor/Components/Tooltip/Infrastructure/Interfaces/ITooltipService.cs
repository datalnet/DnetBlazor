using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Models;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Tooltip.Infrastructure.Interfaces
{
    /// <summary>
    /// Service for displaying and managing tooltips with flexible positioning and content.
    /// </summary>
    public interface ITooltipService
    {
        /// <summary>
        /// Shows a tooltip with text content at the specified element.
        /// </summary>
        /// <param name="tooltipConfig">Configuration for the tooltip appearance and behavior.</param>
        /// <param name="elementReference">The element that the tooltip should be positioned relative to.</param>
        /// <returns>A reference to the overlay containing the tooltip.</returns>
        OverlayReference Show(TooltipConfig tooltipConfig, ElementReference elementReference);

        /// <summary>
        /// Shows a tooltip with a custom component as content.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to display in the tooltip.</typeparam>
        /// <param name="tooltipConfig">Configuration for the tooltip appearance and behavior.</param>
        /// <param name="parameters">Parameters to pass to the component.</param>
        /// <param name="elementReference">The element that the tooltip should be positioned relative to.</param>
        /// <returns>A reference to the overlay containing the tooltip.</returns>
        OverlayReference Show<TComponent>(TooltipConfig tooltipConfig, IDictionary<string, object> parameters, ElementReference elementReference) where TComponent : ComponentBase;

        /// <summary>
        /// Closes a specific tooltip.
        /// </summary>
        /// <param name="overlayDataResult">The result containing the overlay reference ID to close.</param>
        void Close(OverlayResult overlayDataResult);

        /// <summary>
        /// Closes all active tooltips.
        /// </summary>
        void CloseAll();
    }
}
