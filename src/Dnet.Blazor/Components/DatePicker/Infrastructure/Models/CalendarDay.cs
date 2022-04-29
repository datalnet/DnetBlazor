using System;

namespace Dnet.Blazor.Components.DatePicker.Infrastructure.Models
{
    public class CalendarDay
    {
        public int DayNumber { get; set; }

        public string DayName { get; set; }

        public int DayWeek { get; set; }

        public int DayIndex { get; set; }

        public bool IsToday { get; set; } = false;

        public bool IsSelected { get; set; } = false;

        public int Year { get; set; }

        public int Month { get; set; }

        public DateTime Date { get; set; }

        public string FormattedDate { get; set; }
    }
}
