# DnetBlazor
Blazor component library. All components are written in C#.

### Demo
https://www.datalnet.com

### Compatibility
_ Net 7.0.0

- Server-side Blazor and Blazor WASM

### Documentation
No documentation available yet, but many examples in the source code.

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



 
