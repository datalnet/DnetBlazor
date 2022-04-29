using System.Collections.Generic;
using Dnet.Blazor.Components.ImageEditor.Infrastructure.Models;

namespace Dnet.Blazor.Components.ImageEditor.Infrastructure.Services
{
    public interface IImageEditorService
    {
        void UpdateResizersData(ResizerData resizer, double top, double left, double width, double height);

        List<ResizerData> InitializeResizers();

        List<MaskData> PlaceMasks(double imgWidth, double imgHeight, double top, double left, double width, double height);
    }
}
