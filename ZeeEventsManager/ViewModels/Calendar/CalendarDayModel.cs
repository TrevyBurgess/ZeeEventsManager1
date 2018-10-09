//------------------------------------------------------------
// <copyright file="CalendarDayModel.cs" company="KingTrevy" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System.Collections.ObjectModel;

    public class CalendarDayModel : ViewModelBase
    {
        public CalendarDayModel(int day, bool isValidDay, string dayName)
        {
            Day = day;
            IsValidDay = isValidDay;
            DayName = dayName;

            if (string.IsNullOrWhiteSpace(dayName) || dayName.Length < 3)
                ShortDayName = dayName;
            else
                ShortDayName = dayName.Substring(0, 3);
        }

        public ObservableCollection<DayEvent> DayEvents
        {
            get
            {
                return GetState(new ObservableCollection<DayEvent>());
            }

            set
            {
                SetState(value);
            }
        }

        public int Day { get; }

        public bool IsValidDay { get; }

        public string DayName { get; }
        public string ShortDayName { get; }
    }

    public enum EventDurationType
    {
        None,
        SingleDay,
        BeginMultiday,
        MiddleMultiday,
        EndMultiday
    }
}
