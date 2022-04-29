using Dnet.Blazor.Components.ImageEditor.Infrastructure.Enums;

namespace Dnet.Blazor.Components.ImageEditor.Infrastructure.Models
{
    public class DialogData
    {
        public MemoryStream imageFile { get; set; }

        public string ImageUrl { get; set; }

        public MemoryStream WorkingImageStream { get; set; }

        public int ImageContainerHeight { get; set; }

        public int ImageContainerWidth { get; set; }

        public int ImagePreviewHeight { get; set; }

        public int ImagePreviewWidth { get; set; }

        public int ModalDialogHeight { get; set; }

        public int ModalDialogWidth { get; set; }

        public long MaxFileSizes { get; set; }

        public List<string> AllowedFormats { get; set; }

        public List<ImageEditorControlType> ImageEditingControls { get; set; }
    }
}
