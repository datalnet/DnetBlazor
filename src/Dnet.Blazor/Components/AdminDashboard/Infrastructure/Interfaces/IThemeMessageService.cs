using System;
using Dnet.Blazor.Components.AdminDashboard.Infrastructure.Models;

namespace Dnet.Blazor.Components.AdminDashboard.Infrastructure.Interfaces
{
    public interface IThemeMessageService<T>
    {
        event Action<ActionMessage<T>> OnMessage;

        void SendMessage(ActionMessage<T> actionMessage);
    }
}
