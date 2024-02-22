using Dnet.Blazor.Components.FloatingPanel.Infrastructure.Enums;

namespace Dnet.Blazor.Components.FloatingPanel.Infrastructure.Models
{
    public class FloatingPanelConfig
    {
        public string? FloatingPanelClass { get; set; }

        public string? PanelClass { get; set; }

        public bool HasBackdrop { get; set; } = true;

        public bool HasTransparentBackdrop { get; set; }

        public bool DisableBackdropClick { get; set; }

        public string? BackdropClass { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public int Margin { get; set; } = 0;

        public FloatingPanelPostion Postion { get; set; } = FloatingPanelPostion.BottomRight;

        public int? OffsetLeft { get; set; } = 0;

        public int? OffsetRight { get; set; } = 0;

        public int? OffsetTop { get; set; } = 0;

        public int? OffsetBottom { get; set; } = 0;
    }
}
