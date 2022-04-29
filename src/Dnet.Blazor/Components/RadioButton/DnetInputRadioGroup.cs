using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Dnet.Blazor.Infrastructure.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Dnet.Blazor.Components.RadioButton
{
    public class DnetInputRadioGroup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : DnetInputBase<TValue>
    {
        private readonly string _defaultGroupName = Guid.NewGuid().ToString("N");
        private DnetInputRadioContext? _context;

        /// <summary>
        /// Gets or sets the child content to be rendering inside the <see cref="InputRadioGroup{TValue}"/>.
        /// </summary>
        [Parameter] public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        [Parameter] public string? Name { get; set; }

        [CascadingParameter] private DnetInputRadioContext? CascadedContext { get; set; }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            var groupName = !string.IsNullOrEmpty(Name) ? Name : _defaultGroupName;
            //var fieldClass = EditContext?.FieldCssClass(FieldIdentifier) ?? string.Empty;
            var fieldClass = string.Empty;
            var changeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString);

            _context = new DnetInputRadioContext(CascadedContext, groupName, CurrentValue, fieldClass, changeEventCallback);
        }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Debug.Assert(_context != null);

            builder.OpenComponent<CascadingValue<DnetInputRadioContext>>(0);
            builder.SetKey(_context);
            builder.AddAttribute(1, "IsFixed", true);
            builder.AddAttribute(2, "Value", _context);
            builder.AddAttribute(3, "ChildContent", ChildContent);
            builder.CloseComponent();
        }

        /// <inheritdoc />
        protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
            => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
    }
}
