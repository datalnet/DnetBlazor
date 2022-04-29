using System.Globalization;
using Dnet.Blazor.Components.ImageEditor.Infrastructure.Models;
using Dnet.Blazor.Infrastructure.Services.CssBuilder;

namespace Dnet.Blazor.Components.ImageEditor.Infrastructure.Services
{
    public class StyleService : IStyleService
    {
        public string GetStyles(double left, double top, double height, double width)
        {
            var styles = new StyleBuilder("cursor", "auto")
                .AddStyle("position", "absolute")
                .AddStyle("top", $"{top.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("left", $"{left.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("height", $"{height.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("width", $"{width.ToString(CultureInfo.InvariantCulture)}px")
                .Build();

            return styles;
        }

        public string GetResizerStyles(ResizerData resizerData)
        {
            var styles = new StyleBuilder("cursor", resizerData.Cursor)
                .AddStyle("top", $"{resizerData.Top.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("left", $"{resizerData.Left.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("height", $"{resizerData.Height.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("width", $"{resizerData.Width.ToString(CultureInfo.InvariantCulture)}px")
                .Build();

            return styles;
        }

        public string GetMaskStyles(MaskData maskData)
        {
            var styles = new StyleBuilder("top", $"{maskData.Top.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("left", $"{maskData.Left.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("height", $"{maskData.Height.ToString(CultureInfo.InvariantCulture)}px")
                .AddStyle("width", $"{maskData.Width.ToString(CultureInfo.InvariantCulture)}px")
                .Build();

            return styles;
        }

        public string GetCropContainerStyles(int height, int width)
        {
            var styles = new StyleBuilder("position", "absolute")
                .AddStyle("left", "0px")
                .AddStyle("top", "0px")
                .AddStyle("height", $"{height}px")
                .AddStyle("width", $"{width}px")
                .Build();

            return styles;
        }

        public string GetImagePreviewStyles(int height, int width)
        {
            var styles = new StyleBuilder("height", $"{height}px")
                .AddStyle("width", $"{width}px")
                .Build();

            return styles;
        }
    }
}
