using System.Diagnostics.CodeAnalysis;
using Dnet.Blazor.Infrastructure.Forms;
using Dnet.Blazor.Infrastructure.Services.CssBuilder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Dnet.Blazor.Components.Checkbox
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
	public class DnetInputCheckbox : DnetInputBase<bool>
    {
        /// <summary>
        /// Gets or sets the associated <see cref="ElementReference"/>.
        /// <para>
        /// May be <see langword="null"/> if accessed before the component is rendered.
        /// </para>
        /// </summary>
        [DisallowNull] 
        public ElementReference? Element { get; protected set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] 
        public bool Disabled { get; set; }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var s = -1;
            builder.OpenElement(s++, "div");
            builder.AddAttribute(s++, "class", _dnetCheckboxClass);
            builder.OpenElement(s++, "label");
            builder.AddAttribute(s++, "class", "dnet-checkbox-layout");
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-checkbox-inner-container");
            builder.OpenElement(s++, "input");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddAttribute(s++, "type", "checkbox");
            builder.AddAttribute(s++, "disabled", Disabled);
            builder.AddAttribute(s++, "class", $"{CssClass} dnet-checkbox-input cdk-visually-hidden");
            builder.AddAttribute(s++, "checked", BindConverter.FormatValue(CurrentValue));
            builder.AddAttribute(s++, "onchange", EventCallback.Factory.CreateBinder<bool>(this, __value => CurrentValue = __value, CurrentValue));
            builder.AddElementReferenceCapture(6, __inputReference => Element = __inputReference);
            builder.CloseElement();
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-checkbox-frame");
            builder.CloseElement();
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-checkbox-background");
            builder.OpenElement(s++, "svg");
            builder.AddAttribute(s++, "version", "1.1");
            builder.AddAttribute(s++, "focusable", "false");
            builder.AddAttribute(s++, "class", "dnet-checkbox-checkmark");
            builder.AddAttribute(s++, "viewBox", "0 0 24 24");
            builder.AddAttribute(s++, "xml:space", "preserve");
            builder.OpenElement(s++, "path");
            builder.AddAttribute(s++, "class", "dnet-checkbox-checkmark-path");
            builder.AddAttribute(s++, "fill", "none");
            builder.AddAttribute(s++, "stroke", "black");
            builder.AddAttribute(s++, "d", "M4.1,12.7 9,17.6 20.3,6.3");
            builder.CloseElement();
            builder.CloseElement();
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-checkbox-mixedmark");
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "class", "dnet-checkbox-label");
            builder.OpenElement(s++, "span");
            builder.AddAttribute(s++, "style", "display: none");
            builder.CloseElement();
            builder.AddContent(s++, ChildContent);
            builder.CloseElement();
            builder.CloseElement();
            builder.CloseElement();
        }

        private string _dnetCheckboxClass =>
        new CssBuilder("dnet-checkbox dnet-accent")
            .AddClass("dnet-checkbox-checked", CurrentValue == true)
            .AddClass("dnet-checkbox-disabled", Disabled)
        .Build();

        /// <inheritdoc />
        protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
            => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");
    }
}