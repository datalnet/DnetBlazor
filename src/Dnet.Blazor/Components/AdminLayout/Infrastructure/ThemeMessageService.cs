using Dnet.Blazor.Components.AdminLayout.Infrastructure.Interfaces;
using Dnet.Blazor.Components.AdminLayout.Infrastructure.Models;

namespace Dnet.Blazor.Components.AdminLayout.Infrastructure.Services;

public class ThemeMessageService<T>: IThemeMessageService<T>
{
    public event Action<ActionMessage<T>> OnMessage;

    public void SendMessage(ActionMessage<T> actionMessage)
    {
        OnMessage?.Invoke(actionMessage);
    }

}
