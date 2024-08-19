using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Dnet.Blazor.Infrastructure.Services.CssBuilder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Dnet.Blazor.Components.RadioButton
{
    /* This is exactly equivalent to a .razor file containing:
     *
     *    @inherits InputBase<bool>
     *    <input type="checkbox" @bind="CurrentValue" id="@Id" class="@CssClass" />
     *
     * The only reason it's not implemented as a .razor file is that we don't presently have the ability to compile those
     * files within this project. Developers building their own input components should use Razor syntax.
     */

    /// <summary>
    /// An input component for editing <see cref="bool"/> values.
    /// </summary>
    public class DnetInputRadioButton<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : ComponentBase
    {
        /// <summary>
        /// Gets context for this <see cref="InputRadio{TValue}"/>.
        /// </summary>
        internal DnetInputRadioContext? Context { get; private set; }

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the input element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        /// <summary>
        /// Gets or sets the value of this input.
        /// </summary>
        [Parameter]
        public TValue? Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent input radio group.
        /// </summary>
        [Parameter] public string? Name { get; set; }

        [Parameter] public bool TextPlacedBefore { get; set; } = false;

        [CascadingParameter] private DnetInputRadioContext? CascadedContext { get; set; }

        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            Context = string.IsNullOrEmpty(Name) ? CascadedContext : CascadedContext?.FindContextInAncestors(Name);

            if (Context == null)
            {
                throw new InvalidOperationException($"{GetType()} must have an ancestor {typeof(DnetInputRadioGroup<TValue>)} " +
                    $"with a matching 'Name' property, if specified.");
            }
        }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] 
        public bool Disabled { get; set; }


        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Debug.Assert(Context != null);

            var s = -1;
            builder.OpenElement(s++, "div");
            builder.AddAttribute(s++, "class", _dnetRadioButtonClass);
            builder.OpenElement(s++, "label");
			if (TextPlacedBefore)
			{
				builder.AddAttribute(s++, "class", "dnet-radio-label text-before");
			}
			else
			{
				builder.AddAttribute(s++, "class", "dnet-radio-label text-after");
			}
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-radio-container");
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-radio-outer-circle");
            builder.CloseElement();
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-radio-inner-circle");
            builder.CloseElement();
            builder.OpenElement(s++, "input");
            builder.AddMultipleAttributes(s++, AdditionalAttributes);
            builder.AddAttribute(s++, "class", $"dnet-radio-input cdk-visually-hidden");
            builder.AddAttribute(s++, "type", "radio");
            builder.AddAttribute(s++, "disabled", Disabled);
            builder.AddAttribute(s++, "name", Context.GroupName);
            builder.AddAttribute(s++, "value", BindConverter.FormatValue(Value?.ToString()));
            builder.AddAttribute(s++, "checked", Context.CurrentValue?.Equals(Value));
            builder.AddAttribute(s++, "onchange", Context.ChangeEventCallback);
            builder.CloseElement();
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-ripple dnet-radio-ripple dnet-focus-indicator");
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-ripple-element dnet-radio-persistent-ripple");
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-radio-label-content");
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "style", "display: none");
            builder.CloseElement();
            builder.AddContent(s++, ChildContent);
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
        }

        private string _dnetRadioButtonClass =>
        new CssBuilder("dnet-radio-button dnet-accent")
            .AddClass("dnet-radio-checked", (bool)Context.CurrentValue?.Equals(Value))
            .AddClass("dnet-radio-disabled", Disabled)
        .Build();
    }
}