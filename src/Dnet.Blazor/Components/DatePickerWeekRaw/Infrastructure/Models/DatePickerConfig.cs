namespace Dnet.Blazor.Components.DatePickerWeekRaw.Infrastructure.Models;

public class DatePickerConfig
{
    public string? Title { get; set; }

    public string? DialogClass { get; set; }

    public int OverlayReferenceId { get; set; }

    public string? PanelClass { get; set; }

    public bool HasBackdrop { get; set; } = true;

    public bool HasTransparentBackdrop { get; set; }

    public string? BackdropClass { get; set; }

    public string Width { get; set; } = "100%";

    public string Height { get; set; } = "100%";

    public string? MinWidth { get; set; } 

    public string? MinHeight { get; set; }

    public string? MaxWidth { get; set; }
    
    public string? MaxHeight { get; set; }
}
