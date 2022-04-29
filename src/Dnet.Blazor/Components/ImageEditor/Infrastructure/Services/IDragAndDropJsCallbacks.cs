using Dnet.Blazor.Components.ImageEditor.Infrastructure.Models;

namespace Dnet.Blazor.Components.ImageEditor.Infrastructure.Services
{
    public interface IDragAndDropJsCallbacks
    {
        void OnDragEnd(DraggedData draggedData);

        void OnDragStart();

        void OnDrag(DraggedData draggedData);

        void OnResizeEnd(DraggedData draggedData);

        void OnResizeStart();

        void OnResize(DraggedData draggedData);
    }
}
