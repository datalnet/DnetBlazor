using Dnet.Blazor.Components.DatePicker.Infrastructure.Models;

namespace Dnet.Blazor.Components.DatePicker.Infrastructure.Services
{
    public class DatePickerService
    {
        public event Action<List<CalendarDay>> OnUpdateList;

        public event Action<CalendarDay> OnDaySelected;

        public event Action OnResetCalendar;

        public void UdateDays(List<CalendarDay> days)
        {
            OnUpdateList?.Invoke(days);
        }

        public void UpdateSelectedDay(CalendarDay day)
        {
            OnDaySelected?.Invoke(day);
        }

        public void ResetCalendar()
        {
            OnResetCalendar?.Invoke();
        }
    }
}
