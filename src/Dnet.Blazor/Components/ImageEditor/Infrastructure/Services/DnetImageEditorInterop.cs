using Dnet.Blazor.Components.ImageEditor.Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.ImageEditor.Infrastructure.Services
{
    public class DnetImageEditorInterop : IAsyncDisposable
    {
        private const string JsFunctionsPrefix = "dnetimageeditor";

        private readonly IDragAndDropJsCallbacks _owner;

        private readonly IJSRuntime _jsRuntime;

        private DotNetObjectReference<DnetImageEditorInterop>? _selfReference;


        public DnetImageEditorInterop(IDragAndDropJsCallbacks owner, IJSRuntime jsRuntime)
        {
            _owner = owner;
            _jsRuntime = jsRuntime;
            _selfReference = DotNetObjectReference.Create(this);
        }

        public ValueTask<FlexibleConnectedPositionStrategyOrigin> GetBoundingClientRect(ElementReference element)
        {
            return _jsRuntime.InvokeAsync<FlexibleConnectedPositionStrategyOrigin>($"{JsFunctionsPrefix}.getBoundingClientRect", element);
        }

        public void InitializeDragAndDrop(ElementReference draggedContainerElement, ElementReference boardArea, double left, double top)
        {
           _jsRuntime.InvokeAsync<FlexibleConnectedPositionStrategyOrigin>($"{JsFunctionsPrefix}.initializeDragAndDrop", _selfReference, draggedContainerElement, boardArea, left, top);
        }

        public void InitializeResize(List<ResizerData> resizers, double initialLeft, double initialTop, double initialHeight, double initialWidth, double imgWidth, double imgHeight, string resizerType, double resizerMinWidth, double resizerMinHeight)
        {
            _jsRuntime.InvokeAsync<FlexibleConnectedPositionStrategyOrigin>($"{JsFunctionsPrefix}.initializeResize", _selfReference, resizers, initialLeft, initialTop, initialHeight, initialWidth, imgWidth, imgHeight, resizerType, resizerMinWidth, resizerMinHeight);
        }

        [JSInvokable]
        public void OnDragEnd(DraggedData draggedData)
        {
            _owner.OnDragEnd(draggedData);
        }

        [JSInvokable]
        public void OnDragStart()
        {
            _owner.OnDragStart();
        }

        [JSInvokable]
        public void OnDrag(DraggedData draggedData)
        {
            _owner.OnDrag(draggedData);
        }

        [JSInvokable]
        public void OnResizeEnd(DraggedData draggedData)
        {
            _owner.OnResizeEnd(draggedData);
        }

        [JSInvokable]
        public void OnResizeStart()
        {
            _owner.OnResizeStart();
        }

        [JSInvokable]
        public void OnResize(DraggedData draggedData)
        {
            _owner.OnResize(draggedData);
        }

        public async ValueTask DisposeAsync()
        {
            if (_selfReference != null)
            {
                await _jsRuntime.InvokeVoidAsync($"{JsFunctionsPrefix}.dispose", _selfReference);
            }
        }
    }
}
