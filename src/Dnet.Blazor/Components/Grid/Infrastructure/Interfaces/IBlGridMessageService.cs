using System;
using Dnet.Blazor.Components.Grid.Infrastructure.Models;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Interfaces
{
    public interface IBlGridMessageService<T>
    {
        event Action<BlGridActionMessage<T>> OnMessage;

        void SendMessage(BlGridActionMessage<T> actionMessage);
    }
}
