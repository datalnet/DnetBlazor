using Dnet.Blazor.Components.AdminDashboard.Infrastructure.Enums;

namespace Dnet.Blazor.Components.AdminDashboard.Infrastructure.Models
{
    public class ActionMessage<T>
    {
        public ThemeMessageEmitter Emitter { get; set; }
        public T Data { get; set; }
    }
}
