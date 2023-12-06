namespace Dnet.Blazor.Components.Form;

public interface IFormEventService
{
    public event Action<bool> OnError;

    public event Action<bool> OnFocus;

    public event Action<string?> OnCurrentValue;

    public event Action OnClearContent;

    public event Action<FormEventData> OnFormEventRaised;

    public event Action OnSufixContentClicked;

    void RaiseError(bool hasError);

    void RaiseFocus(bool hasFocus);

    void RaiseCurrentValue(string? currentValue);

    void RaiseClearContent();

    void FormRaiseEvent(string error, bool hasFocus, object currentValue);

    void RaiseSufixContentClicked();
}
