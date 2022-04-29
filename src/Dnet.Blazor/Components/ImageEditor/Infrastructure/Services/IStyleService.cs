using Dnet.Blazor.Components.ImageEditor.Infrastructure.Models;

namespace Dnet.Blazor.Components.ImageEditor.Infrastructure.Services
{
    public interface IStyleService
    {
        string GetStyles(double left, double top, double height, double width);

        string GetResizerStyles(ResizerData resizerData);

        string GetMaskStyles(MaskData maskData);

        string GetCropContainerStyles(int height, int width);

        string GetImagePreviewStyles(int height, int width);
    }
}
