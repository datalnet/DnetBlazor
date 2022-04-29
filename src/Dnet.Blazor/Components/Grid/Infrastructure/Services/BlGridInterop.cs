using Dnet.Blazor.Components.Grid.BlgGrid;
using Dnet.Blazor.Components.Grid.Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public static class BlGridInterop<TItem>
    {
        public static ValueTask<ClientRectValues> GetBoundingClientRect(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<ClientRectValues>("blginterop.getBoundingClientRect", element);
        }

        public static ValueTask<double> GetElementScrollLeft(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<double>("blginterop.getElementScrollLeft", element);
        }

        public static ValueTask<int> GetElementScrollWidth(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<int>("blginterop.getElementScrollWidth", element);
        }

        public static ValueTask<int> getHeaderWidth(IJSRuntime jsRuntime, string id)
        {
            return jsRuntime.InvokeAsync<int>("blginterop.getHeaderWidth", id);
        }

        public static ValueTask<bool> AddWindowEventListeners(IJSRuntime jsRuntime, ElementReference element, DotNetObjectReference<BlgGrid<TItem>> dotNetClass)
        {
            return jsRuntime.InvokeAsync<bool>("blginterop.addWindowEventListeners", element, dotNetClass);
        }
    }
}
