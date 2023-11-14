namespace Dnet.Blazor.Components.Dialog.Infrastructure.Models
{
    public class DialogConfig
    {
        public string Title { get; set; }

        public string DialogClass { get; set; }

        public int OverlayReferenceId { get; set; }

        public string PanelClass { get; set; } = null;

        public bool HasBackdrop { get; set; } = true;

        public bool HasTransparentBackdrop { get; set; }

        public bool DisableBackdropClick { get; set; }

        public string BackdropClass { get; set; } = null;

        public string Width { get; set; } = "100%";

        public string Height { get; set; } = "100%";

        public string MinWidth { get; set; } = null;

        public string MinHeight { get; set; } = null;

        public string MaxWidth { get; set; } = null;

        public string MaxHeight { get; set; } = null;
    }
}
