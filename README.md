# DnetBlazor
Blazer component library. All components are written in C#.
Current ver: 5.0.0 - See Changelog for latest improvements

### Demo
https://www.datalnet.com

### Compatibility
Net 10.0
Server-side Blazor and Blazor WASM

### Documentation
No documentation available yet, but many examples in the source code.

## Recent Updates (v5.0.0)

### Tooltip Component
Enhanced with memory management and configurable delays:

- **ShowDelay**: Configure delay before tooltip appears
- **HideDelay**: Configure delay before tooltip hides
- **Memory Leak Prevention**: Automatic cleanup of timers and references
- **Smart Cancellation**: Intelligently handles rapid mouse movements

```csharp
@inject ITooltipService TooltipService

<div @ref="_element" 
     @onmouseover="ShowTooltip"
     @onmouseout="HideTooltip">
    Hover me
</div>

@code {
    private ElementReference _element;
    private OverlayReference _tooltipRef;

    void ShowTooltip()
    {
        var config = new TooltipConfig()
        {
            Text = "I appear after 300ms!",
            ShowDelay = 300,
            HideDelay = 200,
            TooltipColor = "rgba(97,97,97,1)"
        };
        _tooltipRef = TooltipService.Show(config, _element);
    }

    void HideTooltip()
    {
        TooltipService.Close(new OverlayResult { 
            OverlayReferenceId = _tooltipRef.GetOverlayReferenceId() 
        });
    }
}
```

## Using the library
### Installation

1. Install Nuget package Dnet.Blazor
2. Add the following script reference to your Index.html(WASM) or _Host.cshtml (Blazor Server): 

```Html
<script src="_content/Dnet.Blazor/rxjs.min.js"></script>
<script src="_content/Dnet.Blazor/dnet-blazor.js"></script>
```

3. Add the following link reference Index.html (WASM) or _Host.cshtml (Blazor Server): 

```Html
<link href="_content/Dnet.Blazor/dnet-blazor-styles.css" rel="stylesheet" />
```

4. Add the following to the MainLayout.razor

```CSharp
<DnetOverlay BaseZindex="YourZIndexValue"></DnetOverlay>
```
Many of the components in the library are based on the Dnet.Blazor.Overlay component. The overlay provides a way to open floating panels on the screen. Manages positioning, zindex, backdrops, etc. 

BaseZindex: Base z-index use by the Overlay component to display components on the screen. Providing this guarantees that components will open with the correct z-index.

eg.
```CSharp
<DnetOverlay BaseZindex="1100"></DnetOverlay>
```

4. Add the following to the Program.cs
```CSharp
using Dnet.Blazor.Infrastructure.Services;
```
```CSharp
builder.Services.AddDnetBlazor();
```



 
