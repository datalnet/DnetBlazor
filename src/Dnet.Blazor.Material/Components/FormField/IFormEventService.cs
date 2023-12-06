namespace Dnet.Blazor.Material.Components.FormField;

public interface IFormEventService
{
    public event Action<bool> OnError;

    public event Action<bool> OnFocus;

    public event Action<string?> OnCurrentValue;

    public event Action OnClearContent;

    void RaiseError(bool hasError);

    void RaiseFocus(bool hasFocus);

    void RaiseCurrentValue(string? currentValue);

    void RaiseClearContent();

    void FormRaiseEvent(string error, bool hasFocus, object currentValue);
}
