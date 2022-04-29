using System;
using Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Interfaces
{
    public interface IConnectedPanelService
    {
        OverlayReference ToggleMenu(Type componentType, IDictionary<string, object> parameters, ElementReference menuTrigger, ConnectedPanelConfig connectedPanelConfig = null);

        OverlayReference Open(Type componentType, IDictionary<string, object> parameters, ElementReference menuTrigger, ConnectedPanelConfig connectedPanelConfig = null);

        void Close(OverlayResult overlayDataResult);

        void Close();
    }
}
