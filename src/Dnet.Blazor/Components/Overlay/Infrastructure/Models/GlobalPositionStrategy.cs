using Dnet.Blazor.Components.Overlay.Infrastructure.Enums;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class GlobalPositionStrategy
    {
        public string TopOffset { get; set; }

        public string RightOffset { get; set; }

        public string BottomOffset { get; set; }

        public string LeftOffset { get; set; }

        public string WidthOffset { get; set; }

        public string HeightOffset { get; set; }

        public string HorizontallOffset { get; set; }

        public string VerticalOffset { get; set; }

        public OverlayPosition OverlayPosition { get; set; } = OverlayPosition.None;

        public bool CenterHorizontally { get; set; } = true;

        public bool CenterVertically { get; set; } = true;
    }
}
