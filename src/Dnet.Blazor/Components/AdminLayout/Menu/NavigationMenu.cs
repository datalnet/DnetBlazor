using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.AdminLayout.Menu;

public class NavigationMenu
{
    public int MenuId { get; set; }

    public int ParentId { get; set; }

    public bool IsNode { get; set; }

    public string Text { get; set; } = string.Empty;

	public string MinifiedText { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;

	public string LinkClass { get; set; } = string.Empty;

	public bool HasImage { get; set; } = false;

    public string Redirect { get; set; } = string.Empty;

	public string Title { get; set; } = string.Empty;

	public string IconClass { get; set; } = string.Empty;

	public List<NavigationMenu>? Children { get; set; }

	public List<string> Rights { get; set; } = new();

	public RenderFragment? CustomContent { get; set; }
}
