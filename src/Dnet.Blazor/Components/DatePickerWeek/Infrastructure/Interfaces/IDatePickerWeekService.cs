using Dnet.Blazor.Components.DatePicker.Infrastructure.Models;

namespace Dnet.Blazor.Components.DatePickerWeek.Infrastructure.Interfaces;

public interface IDatePickerWeekService
{
    event Action<List<CalendarDay>> OnUpdateDays;

    void UdateList(List<CalendarDay> days);
}
