using Dnet.Blazor.Components.Toast.Infrastructure.Enums;

namespace Dnet.Blazor.Components.Toast.Infrastructure.Models
{
    public class ToastConfig
    {
        public string? Title { get; set; }

        public string? Text { get; set; }

        public ToastType ToastType { get; set; }

        public string? ToastTypeIconClass { get; set; }

        public string? ToastCloseIconClass { get; set; }

        public string? ToastTypeColor { get; set; }

        public string? ToastClass { get; set; }

        public string? PanelClass { get; set; }

        public bool HasBackdrop { get; set; } = true;

        public bool HasTransparentBackdrop { get; set; }

        public string? BackdropClass { get; set; }

        public int Width { get; set; } = 300;

        public int Height { get; set; } = 80;

        public int Margin { get; set; } = 8;

        public ToastPostion ToastPostion { get; set; } = ToastPostion.BottomRight;

        public int? OffsetLeft { get; set; } = 15;

        public int? OffsetRight { get; set; } = 15;

        public int? OffsetTop { get; set; } = 15;

        public int? OffsetBottom { get; set; } = 15;

        public int ExcutionTime { get; set; } = 5;

        public bool ShowExcutionTime { get; set; } = false;
    }
}
