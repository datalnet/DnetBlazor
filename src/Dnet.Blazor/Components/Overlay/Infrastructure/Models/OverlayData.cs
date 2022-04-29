using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class OverlayData
    {
        public RenderFragment Content { get; set; }

        public OverlayConfig OverlayConfig { get; set; }
    }
}
