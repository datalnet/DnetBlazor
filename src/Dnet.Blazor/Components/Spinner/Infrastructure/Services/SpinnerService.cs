using System;
using Dnet.Blazor.Components.Spinner.Infrastructure.Interfaces;

namespace Dnet.Blazor.Components.Spinner.Infrastructure.Services
{
    public class SpinnerService : ISpinnerService
    {
        public event Action<int> OnCounterReceived;

        public void Show()
        {
            UdateCounter(1);
        }

        public void Hide()
        {
            UdateCounter(-1);
        }

        public void UdateCounter(int counter)
        {
            OnCounterReceived?.Invoke(counter);
        }

    }
}