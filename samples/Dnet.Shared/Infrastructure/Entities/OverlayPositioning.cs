namespace Dnet.App.Shared.Infrastructure.Entities
{
    public class OverlayPositioning
    {
        public bool Top { get; set; } = false;

        public bool Bottom { get; set; } = false;

        public bool Left { get; set; } = false;

        public bool Right { get; set; } = false;

        public bool CenterHorizontally { get; set; } = false;

        public bool CenterVertically { get; set; } = false;

        public string TopOffset { get; set; } = null;

        public string BottomOffset { get; set; } = null;

        public string LeftOffset { get; set; } = null;

        public string RightOffset { get; set; } = null;
    }
}
