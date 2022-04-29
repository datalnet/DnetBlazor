using System;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Grid.Infrastructure.Models;

namespace Dnet.Blazor.Components.Grid.Infrastructure.Services
{
    public class BlGridMessageService<T>: IBlGridMessageService<T>
    {
        public event Action<BlGridActionMessage<T>> OnMessage;

        public void SendMessage(BlGridActionMessage<T> actionMessage)
        {
            OnMessage?.Invoke(actionMessage);
        }

    }
}
