using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.ImageEditor.Infrastructure.Models
{
    public class ResizerData
    {
        public string ResizerType { get; set; }

        public double Top { get; set; }

        public double Left { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public string Cursor { get; set; }

        public ElementReference Reference { get; set; }
    }
}
