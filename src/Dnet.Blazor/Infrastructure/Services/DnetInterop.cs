using Dnet.Blazor.Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.AdminDashboard.Infrastructure.Services
{
    public static class DnetInterop
    {
        public static ValueTask<ClientRectValues> GetBoundingClientRect(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<ClientRectValues>("dnetinterop.getBoundingClientRect", element);
        }

        public static ValueTask<double> GetElementScrollLeft(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<double>("dnetinterop.getElementScrollLeft", element);
        }

        public static ValueTask<double> GetElementScrollWidth(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<double>("dnetinterop.getElementScrollWidth", element);
        }

        public static ValueTask<ElementOffsets> GetElementOffsets(IJSRuntime jsRuntime, ElementReference element)
        {
            return jsRuntime.InvokeAsync<ElementOffsets>("dnetinterop.getElementSOffsets", element);
        }

        public static ValueTask<T> GetAsync<T>(IJSRuntime jsRuntime, string key)
            => jsRuntime.InvokeAsync<T>("dnetinterop.get", key);

        public static ValueTask<object> SetAsync(IJSRuntime jsRuntime, string key, object value)
            => jsRuntime.InvokeAsync<object>("dnetinterop.set", key, value);

        public static ValueTask<object> DeleteAsync(IJSRuntime jsRuntime, string key)
            => jsRuntime.InvokeAsync<object>("dnetinterop.delete", key);

    }
}
