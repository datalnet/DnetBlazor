using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Services
{

    public class OverlayService : IOverlayService
    {
        public event Action<RenderFragment, OverlayConfig> OnAttach;

        public event Action<OverlayResult> OnDetach;

        public event Action OnBackdropClicked;

        private List<OverlayReference> _overlayReferences { get; set; } = new List<OverlayReference>();

        private int _sequenceNumber { get; set; } = 0;

        public OverlayReference Attach(RenderFragment overlayContent, OverlayConfig overlayConfig)
        {
            //_sequenceNumber = Enumerable.Range(0, int.MaxValue).Except(_overlayReferences.Select(p => p.OverlayReferenceId)).FirstOrDefault();

            Random rnd = new Random();

            int _sequenceNumber = rnd.Next(1000);

            var overlayReference = new OverlayReference(_sequenceNumber);

            _overlayReferences.Add(overlayReference);

            overlayConfig.OverlayReferenceId = overlayReference.OverlayReferenceId;

            OnAttach?.Invoke(overlayContent, overlayConfig);

            return overlayReference;
        }

        public void Detach(OverlayResult overlayDataResult)
        {
            var item = _overlayReferences.Find(p => p.OverlayReferenceId == overlayDataResult.OverlayReferenceId);

            if(item == null) return;

            _overlayReferences.Remove(item);

            if (!_overlayReferences.Any()) _sequenceNumber = 0;

            OnDetach?.Invoke(overlayDataResult);

            item.CloseOverlayReference(overlayDataResult);
        }

        public void BackdropClicked(OverlayResult overlayDataResult)
        {
            Detach(overlayDataResult);
        }
    }
}
