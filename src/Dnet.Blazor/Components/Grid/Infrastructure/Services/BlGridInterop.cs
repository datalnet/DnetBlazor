using Dnet.Blazor.Components.Grid.BlgGrid;
using Dnet.Blazor.Components.Grid.Infrastructure.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services;

public interface IBlGridInterop<TItem>
{
    ValueTask<ClientRectValues> GetBoundingClientRect(ElementReference element);

    ValueTask<double> GetElementScrollLeft(ElementReference element);

    ValueTask<int> GetElementScrollWidth(ElementReference element);

    ValueTask<int> GetHeaderWidth(string id);

    ValueTask<bool> AddWindowEventListeners(ElementReference element, DotNetObjectReference<BlgGrid<TItem>> dotNetClass);
}

public class BlGridInterop<TItem> : IBlGridInterop<TItem>
{
    private IJSRuntime jsRuntime;

    public BlGridInterop(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public ValueTask<ClientRectValues> GetBoundingClientRect(ElementReference element)
    {
        return this.jsRuntime.InvokeAsync<ClientRectValues>("blginterop.getBoundingClientRect", element);
    }

    public ValueTask<double> GetElementScrollLeft(ElementReference element)
    {
        return this.jsRuntime.InvokeAsync<double>("blginterop.getElementScrollLeft", element);
    }

    public ValueTask<int> GetElementScrollWidth(ElementReference element)
    {
        return this.jsRuntime.InvokeAsync<int>("blginterop.getElementScrollWidth", element);
    }

    public ValueTask<int> GetHeaderWidth(string id)
    {
        return this.jsRuntime.InvokeAsync<int>("blginterop.getHeaderWidth", id);
    }

    public ValueTask<bool> AddWindowEventListeners(ElementReference element, DotNetObjectReference<BlgGrid<TItem>> dotNetClass)
    {
        return this.jsRuntime.InvokeAsync<bool>("blginterop.addWindowEventListeners", element, dotNetClass);
    }
}


//public static class BlGridInterop<TItem>
//{
//    public static ValueTask<ClientRectValues> GetBoundingClientRect(IJSRuntime jsRuntime, ElementReference element)
//    {
//        return jsRuntime.InvokeAsync<ClientRectValues>("blginterop.getBoundingClientRect", element);
//    }

//    public static ValueTask<double> GetElementScrollLeft(IJSRuntime jsRuntime, ElementReference element)
//    {
//        return jsRuntime.InvokeAsync<double>("blginterop.getElementScrollLeft", element);
//    }

//    public static ValueTask<int> GetElementScrollWidth(IJSRuntime jsRuntime, ElementReference element)
//    {
//        return jsRuntime.InvokeAsync<int>("blginterop.getElementScrollWidth", element);
//    }

//    public static ValueTask<int> getHeaderWidth(IJSRuntime jsRuntime, string id)
//    {
//        return jsRuntime.InvokeAsync<int>("blginterop.getHeaderWidth", id);
//    }

//    public static ValueTask<bool> AddWindowEventListeners(IJSRuntime jsRuntime, ElementReference element, DotNetObjectReference<BlgGrid<TItem>> dotNetClass)
//    {
//        return jsRuntime.InvokeAsync<bool>("blginterop.addWindowEventListeners", element, dotNetClass);
//    }
//}
