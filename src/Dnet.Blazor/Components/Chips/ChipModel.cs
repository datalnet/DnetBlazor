using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Chips
{
	public class ChipModel
    {
		public RenderFragment ChildContent { get; set; }

		public string BackgroungColor { get; set; }

		public string Color { get; set; }

		public bool Selectable { get; set; } = true;

		public bool Removable { get; set; } = true;

		public string Value { get; set; }

		public ChipSize ChipSize { get; set; } = ChipSize.Small;

		public bool Disabled { get; set; } = false;

		internal int Id { get; set; } = 0;
	}
}
