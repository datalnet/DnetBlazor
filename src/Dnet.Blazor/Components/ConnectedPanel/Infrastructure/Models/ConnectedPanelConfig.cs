namespace Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Models
{
    public class ConnectedPanelConfig
    {
        public string ConnectedPanelClasses { get; set; }

        public int OverlayReferenceId { get; set; }

        public bool HasBackdrop { get; set; } = true;

        public bool HasTransparentBackdrop { get; set; } = true;

        public string BackdropClass { get; set; } = null;

        public string Width { get; set; } = "100%";

        public string Height { get; set; } = "100%";

        public string MinWidth { get; set; } = null;

        public string MinHeight { get; set; } = null;

        public string MaxWidth { get; set; } = null;

        public string MaxHeight { get; set; } = null;

    }
}
