using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Form;

internal class FormFieldCascadingValues
{
    public IFormEventService? FormEventService { get; set; }

    public ElementReference FormField { get; set; }
}
