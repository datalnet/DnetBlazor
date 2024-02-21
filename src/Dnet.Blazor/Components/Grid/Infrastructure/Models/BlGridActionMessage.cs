using Dnet.Blazor.Components.Grid.Infrastructure.Enums;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Models
{
    public class BlGridActionMessage<T>
    {
        public BlGridMessageEmitter Emitter { get; set; }

        public T? Data { get; set; }

        public long RowNodeId { get; set; }

        public int Type { get; set; }
    }
}
