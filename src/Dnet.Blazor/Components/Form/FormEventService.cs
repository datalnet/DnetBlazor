namespace Dnet.Blazor.Components.Form;

public class FormEventService : IFormEventService
{
    public event Action<bool> OnError;

    public event Action<bool> OnFocus;

    public event Action<string?> OnCurrentValue;

    public void RaiseError(bool hasError)
    {
        OnError?.Invoke(hasError);
    }

    public void RaiseFocus(bool hasFocus)
    {
        OnFocus?.Invoke(hasFocus);
    }

    public void RaiseCurrentValue(string? currentValue)
    {
        OnCurrentValue?.Invoke(currentValue);
    }
}
