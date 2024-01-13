using Dnet.Blazor.Components.AdminLayout.Infrastructure.Enums;

namespace Dnet.Blazor.Components.AdminLayout.Infrastructure.Models;

public class ActionMessage<T>
{
    public ThemeMessageEmitter Emitter { get; set; }

    public T Data { get; set; }
}
