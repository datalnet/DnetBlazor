using Dnet.Blazor.Components.AdminLayout.Infrastructure.Models;

namespace Dnet.Blazor.Components.AdminLayout.Infrastructure.Interfaces;

public interface IThemeMessageService<T>
{
    event Action<ActionMessage<T>> OnMessage;

    void SendMessage(ActionMessage<T> actionMessage);
}
