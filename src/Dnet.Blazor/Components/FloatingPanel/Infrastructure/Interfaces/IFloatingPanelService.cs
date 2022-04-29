using System;
using System.Collections.Generic;
using Dnet.Blazor.Components.FloatingPanel.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;

namespace Dnet.Blazor.Components.FloatingPanel.Infrastructure.Interfaces
{
    public interface IFloatingPanelService
    {
        OverlayReference Show(Type componentType, IDictionary<string, object> parameters, FloatingPanelConfig floatingPanelConfig);

        void Close(OverlayResult overlayDataResult);
    }
}
