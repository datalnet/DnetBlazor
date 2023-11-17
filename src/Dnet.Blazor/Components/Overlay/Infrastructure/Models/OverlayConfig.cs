using Dnet.Blazor.Components.Overlay.Infrastructure.Enums;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class OverlayConfig
    {
        public int OverlayReferenceId { get; set; }

        public string PanelClass { get; set; }

        internal int? PanelZindex { get; set; }

        internal int? HostZindex { get; set; }

        public bool HasBackdrop { get; set; } = true;

        public bool HasTransparentBackdrop { get; set; }

        public bool DisableBackdropClick { get; set; }

        public string BackdropClass { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public string MinWidth { get; set; }

        public string MinHeight { get; set; }

        public string MaxWidth { get; set; }

        public string MaxHeight { get; set; }

        public string MarginTop { get; set; }

        internal int? LastZindex { get; set; } = 0;

        public PositionStrategy PositionStrategy { get; set; } = PositionStrategy.Global;

        public GlobalPositionStrategyBuilder GlobalPositionStrategy { get; set; }

        public FlexibleConnectedPositionStrategyBuilder FlexibleConnectedPositionStrategyBuilder { get; set; }
    }
}
