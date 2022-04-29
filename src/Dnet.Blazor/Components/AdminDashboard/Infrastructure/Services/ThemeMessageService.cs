using System;
using Dnet.Blazor.Components.AdminDashboard.Infrastructure.Interfaces;
using Dnet.Blazor.Components.AdminDashboard.Infrastructure.Models;

namespace Dnet.Blazor.Components.AdminDashboard.Infrastructure.Services
{
    public class ThemeMessageService<T>: IThemeMessageService<T>
    {
        public event Action<ActionMessage<T>> OnMessage;

        public void SendMessage(ActionMessage<T> actionMessage)
        {
            OnMessage?.Invoke(actionMessage);
        }

    }
}
