namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class OverlayFit
    {
        public bool IsCompletelyWithinViewport { get; set; }

        public bool FitsInViewportVertically { get; set; }

        public bool FitsInViewportHorizontally { get; set; }

        public double? VisibleArea { get; set; }
    }
}
