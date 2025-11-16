# Changelog for Blazor Library

## Version 5.0.0 (November 2025)

### Tooltip Component Improvements

#### Memory Management
- **IDisposable Implementation**: TooltipService now properly implements IDisposable pattern for automatic resource cleanup
- **Timer Management**: All show and hide timers are now properly tracked and disposed
- **Reference Tracking**: Active tooltips are tracked in dictionaries to prevent memory leaks
- **Automatic Cleanup**: Dispose() method ensures all resources are released when the service is disposed

#### Show/Hide Delays
- **ShowDelay Property**: Tooltips can now be configured to appear after a specified delay (in milliseconds)
- **HideDelay Property**: Tooltips can now be configured to hide after a specified delay when the mouse leaves
- **Smart Cancellation**: 
  - If mouse leaves before ShowDelay completes, tooltip creation is cancelled
  - If mouse re-enters during HideDelay, the hide operation is cancelled
- **Thread-Safe Operations**: All timer operations are protected with locks for concurrent access

#### Technical Improvements
- **ID Mapping System**: Placeholder IDs are mapped to real overlay IDs for proper tracking with delayed tooltips
- **Unified Show Logic**: Internal ShowInternal() method reduces code duplication
- **Improved Close Logic**: Separate CloseImmediate() method for immediate cleanup vs delayed closing

#### Usage Example
```csharp
var tooltipConfig = new TooltipConfig()
{
    Text = "This tooltip appears after 500ms",
    ShowDelay = 500,  // Wait 500ms before showing
    HideDelay = 200   // Wait 200ms before hiding
};

_tooltipReference = TooltipService.Show(tooltipConfig, _element);

// When mouse leaves
TooltipService.Close(new OverlayResult { OverlayReferenceId = _tooltipReference.GetOverlayReferenceId() });
```

## Upcoming Changes v.4.0.0
- **Default Theme Update**: Form components will now default to the 'Plain' theme.
- **Separate NuGet Package for Material Theme**: Users wishing to use the Material theme can find it in a separate NuGet package, available from January 2024.

## Recommended Migration Path
- **Version 3.2.0 as a Transition Release**: Users are advised to use version 3.2.0 as an intermediary step when migrating from version 3.2.0 to 4+.
  - This version (3.2.0) includes both Material and Plain themes in the same package, facilitating a smoother transition to later versions where these themes are separated.
