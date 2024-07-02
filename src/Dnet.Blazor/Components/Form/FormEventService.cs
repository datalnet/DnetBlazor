namespace Dnet.Blazor.Components.Form;

public class FormEventService : IFormEventService
{
    public event Action<bool>? OnError;

    public event Action<bool>? OnFocus;

    public event Action<string?>? OnCurrentValue;

    public event Action? OnClearContent;

    public event Action<FormEventData>? OnFormEventRaised;

    public event Action? OnSufixContentClicked;

    public event Action<bool>? OnDisabled;

    public event Action<Tuple<int, int>>? OnCharCount;

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

    public void RaiseClearContent()
    {
        OnClearContent?.Invoke();
    }

    public void FormRaiseEvent(string error, bool hasFocus, object currentValue)
    {
        var eventData = new FormEventData(error, hasFocus, currentValue);

        OnFormEventRaised?.Invoke(eventData);
    }

    public void RaiseSufixContentClicked()
    {
        OnSufixContentClicked?.Invoke();
    }

    public void RaiseDisabledEvent(bool isDisabled)
    {
        OnDisabled?.Invoke(isDisabled);
    }

    public void RaiseCharCountEvent(Tuple<int, int> countData)
    {
        OnCharCount?.Invoke(countData);
    }
}

public class FormEventData
{
    public string Error { get; set; }

    public bool HasFocus { get; set; }

    public object CurrentValue { get; set; }

    public FormEventData(string error, bool hasFocus, object currentValue)
    {
        Error = error;
        HasFocus = hasFocus;
        CurrentValue = currentValue;
    }
}
