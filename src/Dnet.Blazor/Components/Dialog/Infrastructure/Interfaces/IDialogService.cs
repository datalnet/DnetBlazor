using System;
using System.Collections.Generic;
using Dnet.Blazor.Components.Dialog.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;

namespace Dnet.Blazor.Components.Dialog.Infrastructure.Interfaces
{
    public interface IDialogService
    {
        OverlayReference Open(Type componentType, IDictionary<string, object> parameters, DialogConfig dialogConfig);

        void Close(OverlayResult overlayDataResult);
    }
}
