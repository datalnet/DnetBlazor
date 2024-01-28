namespace Dnet.Blazor.Components.AdminLayout.Infrastructure.Models;

public class ThemeConfigData
{
    public bool IsDesktopMode { get; set; } = true;

    public bool IsFooterHidden { get; set; } = true;

    public bool IsHeaderHidden { get; set; } = false;

    public bool IsLeftColumnHidden { get; set; } = false;

    public bool IsMinified { get; set; } = false;

    public bool IsHeaderFixed { get; set; } = false;

    public bool IsLeftColumnFixed { get; set; } = false;

    public bool ShowMinifier { get; set; } = true;
}
