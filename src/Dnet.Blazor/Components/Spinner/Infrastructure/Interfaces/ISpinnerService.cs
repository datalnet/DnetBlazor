namespace Dnet.Blazor.Components.Spinner.Infrastructure.Interfaces
{
    public interface ISpinnerService
    {
        event Action<int> OnCounterReceived;

        void Show();

        void Hide();

        void UdateCounter(int items);
    }
}
