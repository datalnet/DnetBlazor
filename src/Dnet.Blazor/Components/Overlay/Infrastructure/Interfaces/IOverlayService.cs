using System;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces
{
    public interface IOverlayService
    {
        event Action OnBackdropClicked;

        OverlayReference Attach(RenderFragment overlayContent, OverlayConfig overlayConfig);

        void Detach(OverlayResult overlayDataResult);

        void BackdropClicked(OverlayResult overlayDataResult);
    }
}
