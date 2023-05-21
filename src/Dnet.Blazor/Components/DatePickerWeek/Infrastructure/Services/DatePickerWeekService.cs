using Dnet.Blazor.Components.DatePickerWeek.Infrastructure.Models;

namespace Dnet.Blazor.Components.DatePickerWeek.Infrastructure.Services;

public class DatePickerWeekService
{
    public event Action<List<CalendarDay>> OnUpdateList;

    public event Action<List<CalendarDay>> OnDaySelected;

    public void UdateDays(List<CalendarDay> days)
    {
        OnUpdateList?.Invoke(days);
    }

    public void UpdateSelectedDay(List<CalendarDay> days)
    {
        OnDaySelected?.Invoke(days);
    }
}
