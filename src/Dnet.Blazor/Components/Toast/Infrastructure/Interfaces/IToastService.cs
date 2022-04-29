using System;
using System.Collections.Generic;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Toast.Infrastructure.Models;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Toast.Infrastructure.Interfaces
{
    public interface IToastService
    {
        void Show(ToastConfig toastConfig, Type componentType = null, IDictionary<string, object> parameters = null, RenderFragment dialogContent = null);

        void Close(OverlayResult overlayDataResult);
    }
}
