namespace Dnet.Blazor.Components.Tooltip.Infrastructure.Models
{
    public class TooltipConfig
    {
        public string? Text { get; set; }

        public string? TooltipClass { get; set; }

        public string? TooltipColor { get; set; }

        public string? Width { get; set; }

        public string? Height { get; set; }

        public string? MinWidth { get; set; }

        public string? MinHeight { get; set; }

        public string? MaxWidth { get; set; }

        public string? MaxHeight { get; set; }

        /// <summary>
        /// Delay in milliseconds before showing the tooltip. Default is 0 (immediate).
        /// </summary>
        public int ShowDelay { get; set; } = 0;

        /// <summary>
        /// Delay in milliseconds before hiding the tooltip. Default is 0 (immediate).
        /// </summary>
        public int HideDelay { get; set; } = 0;
    }
}
