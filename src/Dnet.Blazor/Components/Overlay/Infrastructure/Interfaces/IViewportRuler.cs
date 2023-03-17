using System;
using System.Threading.Tasks;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces
{
    public interface IViewportRuler
    {
        Task<Models.Size> GetViewportSize();

        Task<ClientRect> GetViewportRect();

        Task<ViewportScrollPosition> GetViewportScrollPosition();

        Task<ClientRect> GetViewportRectNoScroll(int viewportMargin);

        void OnWindowResized(Models.Size size);

        event EventHandler<Models.Size> OnResized;

        ValueTask<bool> AddWindowEventListeners();

        ValueTask RemoveWindowEventListeners();

    }
}
