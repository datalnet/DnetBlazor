using System;
using System.Collections.Generic;
using Dnet.Blazor.Components.DatePicker.Infrastructure.Models;

namespace Dnet.Blazor.Components.DatePicker.Infrastructure.Interfaces
{
    public interface IDatePickerService
    {
        event Action<List<CalendarDay>> OnUpdateDays;

        void UdateList(List<CalendarDay> days);
    }
}
