// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Threading.Tasks;

namespace Dnet.Blazor.Components.Grid.Virtualize
{
    internal interface IVirtualizeJsCallbacks
    {
        Task OnBeforeSpacerVisible(float spacerSize, float spacerSeparation, float containerSize);

        Task OnAfterSpacerVisible(float spacerSize, float spacerSeparation, float containerSize);
    }
}
