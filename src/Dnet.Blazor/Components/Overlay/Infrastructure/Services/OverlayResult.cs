using Dnet.Blazor.Components.Overlay.Infrastructure.Enums;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Services
{
    public class OverlayResult
    {

        public CloseReason CloseReason { get; set; }

        public object ComponentData { get; set; }

        public int OverlayReferenceId { get; set; }

        public string CurrentAction { get; set; }
    }
}
