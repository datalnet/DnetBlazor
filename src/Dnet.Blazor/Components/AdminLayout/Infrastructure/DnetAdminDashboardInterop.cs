using Dnet.Blazor.Components.AdminLayout.Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.AdminLayout.Infrastructure.Services;

public static class DnetAdminDashboardInterop
{
    public static ValueTask<ClientRectValues> GetBoundingClientRect(IJSRuntime jsRuntime, ElementReference element)
    {
        return jsRuntime.InvokeAsync<ClientRectValues>("admindashboardinterop.getBoundingClientRect", element);
    }

    public static ValueTask<double> GetElementScrollLeft(IJSRuntime jsRuntime, ElementReference element)
    {
        return jsRuntime.InvokeAsync<double>("admindashboardinterop.getElementScrollLeft", element);
    }

    public static ValueTask<int> GetElementScrollWidth(IJSRuntime jsRuntime, ElementReference element)
    {
        return jsRuntime.InvokeAsync<int>("admindashboardinterop.getElementScrollWidth", element);
    }

    public static ValueTask<ElementOffsets> GetElementOffsets(IJSRuntime jsRuntime, ElementReference element)
    {
        return jsRuntime.InvokeAsync<ElementOffsets>("admindashboardinterop.getElementSOffsets", element);
    }

    public static ValueTask<T> GetAsync<T>(IJSRuntime jsRuntime, string key)
        => jsRuntime.InvokeAsync<T>("admindashboardinterop.get", key);

    public static ValueTask<object> SetAsync(IJSRuntime jsRuntime, string key, object value)
        => jsRuntime.InvokeAsync<object>("admindashboardinterop.set", key, value);

    public static ValueTask<object> DeleteAsync(IJSRuntime jsRuntime, string key)
        => jsRuntime.InvokeAsync<object>("admindashboardinterop.delete", key);

}
