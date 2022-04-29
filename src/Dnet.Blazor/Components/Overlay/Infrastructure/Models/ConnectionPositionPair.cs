using Dnet.Blazor.Components.Overlay.Infrastructure.Enums;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class ConnectionPositionPair
    {
        public HorizontalConnectionPos OriginX { get; set; }

        public VerticalConnectionPos OriginY { get; set; }

        public HorizontalConnectionPos OverlayX { get; set; }

        public VerticalConnectionPos OverlayY { get; set; }
    }
}
