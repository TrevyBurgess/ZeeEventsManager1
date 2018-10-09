//------------------------------------------------------------
// <copyright file="CalendarModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public class CalendarModel : PageViewmodelBase<AppLevelModel>
    {
        public CalendarModel(AppLevelModel appViewModel) : base(appViewModel)
        {
            MainMenu = appViewModel.TopLevelMenu;
            MainMenuIndex = 0;
            IsInitialAppPage = true;
            IconCode = "Home";
            PageTitle = Q.Resources.Calendar_PageTitle;

            //SubMenu.Add(new MenuItem<AppLevelModel>(
            //    this,
            //    typeof(CalendarMonthPage),
            //    null,
            //    null,
            //    "Month", "Calendar", true));
            //SubMenu.Add(new MenuItem<AppLevelModel>(
            //    this,
            //    typeof(CalendarWeekPage),
            //    null,
            //    null,
            //    "Week", "CalendarWeek", true));
            //SubMenu.Add(new MenuItem<AppLevelModel>(
            //    this,
            //    typeof(CalendarDayPage),
            //    null,
            //    null,
            //    "Day", "CalendarDay", true));

            // New event command
            SubMenu.Add(NewEventCommand = new MenuItem<AppLevelModel>(
                null,
                typeof(EventItemPage),
                null,
                NewEventBeforeCommand,
                Q.Resources.Calendar_AddNewEvent, "Add"));
            SubMenuIndex = -1;

            BottomMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(appViewModel.Settings.AboutViewModel, typeof(AboutPage) )
            };
            BottomMenuIndex = -1;
            ResetBottomMenuIndexOnNavigation = true;

            var endMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1).AddMonths(1);
        }

        /// <summary>
        /// Command to create new event
        /// </summary>
        public MenuItem<AppLevelModel> NewEventCommand { get; }

        public override string SearchTerm { get; set; }

        public string PreviousMonthName
        {
            get
            {
                return GetState<string>();
            }
            set
            {
                SetState(value);
            }
        }

        public string CurrentMonth
        {
            get
            {
                return GetState<string>();
            }
            set
            {
                SetState(value);
            }
        }

        public DateTime CurrentDate
        {
            get
            {
                return GetState(DateTime.MinValue);
            }
            set
            {
                var date = new DateTime(
                    value.Year,
                    value.Month,
                    value.Day,
                    value.Hour,
                    value.Minute < 30 ? 0 : 30,
                    0);

                if (SetState(date))
                {
                    var prevMonth = value.AddMonths(-1);
                    PreviousMonthName = prevMonth.ToString("MMMM") + ", " + prevMonth.Year;

                    CurrentMonth = value.ToString("MMMM") + ", " + CurrentDate.Year;

                    var nextMonth = value.AddMonths(1);
                    NextMonthName = nextMonth.ToString("MMMM") + ", " + nextMonth.Year;

                    FirstDayOfMonth = new DateTime(value.Year, value.Month, 1);
                    LastDayOfMonth = FirstDayOfMonth.AddMonths(1).AddDays(-1);

                    CalendarDays = SqLiteManager.GetCalendarMonth(
                        CurrentDate.Year,
                        CurrentDate.Month,
                        AppViewModel);
                }
            }
        }

        /// <summary>
        /// Each month contains a list of days. Each day has events.
        /// </summary>
        public ObservableCollection<CalendarDayModel> CalendarDays
        {
            get
            {
                return GetState(new ObservableCollection<CalendarDayModel>());
            }
            set
            {
                SetState(value);
            }
        }

        public DateTime FirstDayOfMonth
        {
            get
            {
                return GetState<DateTime>();
            }

            set
            {
                SetState(value);
            }
        }

        public DateTime LastDayOfMonth
        {
            get
            {
                return GetState<DateTime>();
            }

            set
            {
                SetState(value);
            }
        }

        public string NextMonthName
        {
            get
            {
                return GetState<string>();
            }
            set
            {
                SetState(value);
            }
        }

        public string[] DayNames
        {
            get
            {
                return Enum.GetNames(typeof(DayOfWeek));
            }
        }

        public RelayCommand PreviousMonth
        {
            get
            {
                return Command(() =>
                {
                    CurrentDate = CurrentDate.AddMonths(-1);
                });
            }
        }

        public RelayCommand NextMonth
        {
            get
            {
                return Command(() =>
                {
                    CurrentDate = CurrentDate.AddMonths(1);
                });
            }
        }

        private RelayCommand NewEventBeforeCommand
        {
            get
            {
                return Command(() =>
                {
                    int day;
                    if (CurrentDate.Year == DateTime.Now.Year && CurrentDate.Month == DateTime.Now.Month)
                        day = CurrentDate.Day;
                    else
                        day = 1;
                    var date = new DateTime(
                            CurrentDate.Year,
                            CurrentDate.Month,
                            day,
                            DateTime.Now.Hour,
                            DateTime.Now.Minute < 30 ? 0 : 30,
                            0);

                    var newEvent = NewEventItemModel.GetNewEvent(date, true);

                    NewEventCommand.ViewModel = newEvent;
                });
            }
        }

        /// <summary>
        /// Add event to calendar for display
        /// </summary>
        public void AddOrUpdateEvent(BaseEventItemModel theEventItemModel)
        {
            EventItemModel eventItemModel;
            if (theEventItemModel is NewEventItemModel)
            {
                SqLiteManager.SaveNewEvent(theEventItemModel as NewEventItemModel);
                eventItemModel = ConvertEvent(theEventItemModel as NewEventItemModel);
            }
            else
            {
                SqLiteManager.UpdateExistingEvent(theEventItemModel as EventItemModel);
                eventItemModel = theEventItemModel as EventItemModel;
                HideEvent(eventItemModel as EventItemModel);
            }

            if (!(eventItemModel.EventBegin < FirstDayOfMonth || eventItemModel.EventEnd > LastDayOfMonth.AddDays(1)))
            {
                int offset = SqLiteManager.GetCalendarMonthOffset(CurrentDate.Month, CurrentDate.Year);

                int start;
                if (eventItemModel.EventBegin < FirstDayOfMonth)
                    start = 1;
                else
                    start = eventItemModel.EventBegin.Day;

                int end;
                if (eventItemModel.EventEnd > LastDayOfMonth)
                    end = LastDayOfMonth.Day;
                else
                    end = eventItemModel.EventEnd.Day;

                if (start == end)
                {
                    CalendarDays[start + offset - 1].
                        DayEvents.
                        Add(new DayEvent(eventItemModel, EventDurationType.None));
                }
                else
                {
                    for (var day = start; day <= end; day++)
                    {
                        DayEvent dayEvent;
                        if (day == start)
                            dayEvent = new DayEvent(eventItemModel, EventDurationType.BeginMultiday);
                        else if (day == end)
                            dayEvent = new DayEvent(eventItemModel, EventDurationType.EndMultiday);
                        else
                            dayEvent = new DayEvent(eventItemModel, EventDurationType.MiddleMultiday);

                        CalendarDays[day + offset].DayEvents.Add(dayEvent);
                    }
                }
            }
        }

        /// <summary>
        /// Should be called from event to be deleted.
        /// </summary>
        /// <param name="newEvent"></param>
        public void DeleteEvent(BaseEventItemModel eventToDelete)
        {
            SqLiteManager.DeleteEvent(eventToDelete);
            HideEvent(eventToDelete);
        }

        private void HideEvent(BaseEventItemModel eventToHide)
        {
            // Find and delete event from collection
            foreach (var day in CalendarDays)
            {
                foreach (var event1 in day.DayEvents)
                {
                    if (event1.Event.ID == eventToHide.ID)
                    {
                        day.DayEvents.Remove(event1);
                        break;
                    }
                }
            }
        }

        public override Task<bool> OnNavigatingFrom()
        {
            return Task.Run(() => { return true; });
        }

        private EventItemModel ConvertEvent(NewEventItemModel newEvent)
        {
            var convertedEvent = new EventItemModel(AppViewModel, newEvent.EventBegin, newEvent.NavFromCalendar)
            {
                EventTitle = newEvent.EventTitle,
                Venue = newEvent.Venue,
                Description = newEvent.Description,
                EventBegin = newEvent.EventBegin,
                EventEnd = newEvent.EventEnd,
                ImagePath = newEvent.ImagePath,
                VenueContactEmail = newEvent.VenueContactEmail,
                VenueContactPhone = newEvent.VenueContactPhone,
                ID = newEvent.ID
            };

            convertedEvent.GuestListModel = newEvent.GuestListModel;

            return convertedEvent;
        }
    }
}
