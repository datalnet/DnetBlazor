// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Dnet.Blazor.Components.Grid.Virtualize
{
    internal class VirtualizeJsInterop : IAsyncDisposable
    {
        private const string JsFunctionsPrefix = "blginterop";

        private readonly IVirtualizeJsCallbacks _owner;

        private readonly IJSRuntime _jsRuntime;

        private DotNetObjectReference<VirtualizeJsInterop>? _selfReference;

        public VirtualizeJsInterop(IVirtualizeJsCallbacks owner, IJSRuntime jsRuntime)
        {
            _owner = owner;
            _jsRuntime = jsRuntime;
        }

        public async ValueTask InitializeAsync(ElementReference spacerBefore, ElementReference spacerAfter, int rootMargin = 50)
        {
            _selfReference = DotNetObjectReference.Create(this);
            await _jsRuntime.InvokeVoidAsync($"{JsFunctionsPrefix}.init", _selfReference, spacerBefore, spacerAfter, rootMargin);
        }

        [JSInvokable]
        public void OnSpacerBeforeVisible(float spacerSize, float spacerSeparation, float containerSize)
        {
            _owner.OnBeforeSpacerVisible(spacerSize, spacerSeparation, containerSize);
        }

        [JSInvokable]
        public void OnSpacerAfterVisible(float spacerSize, float spacerSeparation, float containerSize)
        {
            _owner.OnAfterSpacerVisible(spacerSize, spacerSeparation, containerSize);
        }

        public async ValueTask DisposeAsync()
        {
            if (_selfReference != null)
            {
                try
                {
                    await _jsRuntime.InvokeVoidAsync($"{JsFunctionsPrefix}.dispose", _selfReference);
                }
                catch (JSDisconnectedException)
                {
                    // If the browser is gone, we don't need it to clean up any browser-side state
                }
            }
        }
    }
}
