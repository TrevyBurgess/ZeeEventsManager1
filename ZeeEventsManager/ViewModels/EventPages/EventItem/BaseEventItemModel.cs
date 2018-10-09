//------------------------------------------------------------
// <copyright file="BaseEventItemModel.cs" company="TrevyBurgess" >
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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.Devices.Geolocation;
    using Windows.Services.Maps;
    using Windows.UI;
    using Windows.UI.Xaml.Media;

    public abstract class BaseEventItemModel : PageViewmodelBase<AppLevelModel>
    {
        public event SearchTermChangedHandler SearchTermChanged;

        public bool NavFromCalendar { get; }

        public BaseEventItemModel(AppLevelModel appViewModel, DateTime eventStartDate, bool navFromCalendar) : base(appViewModel)
        {
            NavFromCalendar = navFromCalendar;
            SetMainMenu();

            GuestListModel = new EventGuestListModel(appViewModel, this);

            MapCenter = new Geopoint(new BasicGeoposition()
            {
                // Geopoint for Seattle
                Latitude = 47.604,
                Longitude = -122.329
            });

            BottomMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(appViewModel.Settings.AboutViewModel, typeof(AboutPage) )
            };
            BottomMenuIndex = -1;
            ResetBottomMenuIndexOnNavigation = true;

            EventBegin = eventStartDate;
            EventEnd = eventStartDate;
            EditedGuestList = new HashSet<int>();

            Settings = appViewModel.Settings;
        }

        public SettingsModel Settings { get; private set; }

        protected void SetMainMenu()
        {
            // Set to 0 to force index change
            MainMenuIndex = 0;
            if (NavFromCalendar)
            {
                MainMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
                {
                    new MenuItem<AppLevelModel>(Q.Resources.Event_ReturnToCalendar, "ImportAll")
                };
                MainMenuIndex = -1;
                ResetMainMenuIndexOnNavigation = true;
            }
            else
            {
                MainMenu = AppViewModel.TopLevelMenu;
                MainMenuIndex = 1;
                ResetMainMenuIndexOnNavigation = false;
            }

            SubMenu.Clear();
            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditAttendance, "Contact2", EditAttendanceCommand));
            if (EditEvent)
            {
                SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_SaveEvent, "Save", SaveEventCommand));
                SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_DiscardChanges, "Cancel", CancelEventChangesCommand));
            }
            else
            {
                SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditEvent, "Edit", ViewEventCommand));
                SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_DeleteEvent, "Delete", DeleteEventCommand));
            }
        }

        #region Event values
        public int ID
        {
            get
            {
                return GetState(-1);
            }
            set
            {
                SetState(value);
            }
        }

        public string EventTitle
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                if (SetState(value))
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        if (EventEnd.AddDays(1) < DateTime.Now)
                        {
                            PageTitle = Q.Resources.Event_Placeholder_SetTitle_PastEvent;
                        }
                        else
                        {
                            PageTitle = Q.Resources.Event_Placeholder_SetTitle;
                        }
                    }
                    else
                    {
                        if (EventEnd.AddDays(1) < DateTime.Now)
                        {
                            PageTitle = string.Format(Q.Resources.Event_Placeholder_Titled_PastEvent, EventTitle);
                        }
                        else
                        {
                            PageTitle = EventTitle;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Color to paint event in calendar
        /// </summary>
        public SolidColorBrush EventColor
        {
            get
            {
                return GetState(new SolidColorBrush(Colors.Black));
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Color to paint event in calendar
        /// </summary>
        public SolidColorBrush EventColorBackground
        {
            get
            {
                return GetState(new SolidColorBrush(Colors.White));
            }
            set
            {
                SetState(value);
            }
        }

        public string Venue
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                if (SetState(value))
                    SetMapCenter();
            }
        }

        public string Description
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SetState(value);
            }
        }

        public string VenueContactEmail
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SetState(value);
            }
        }

        public string VenueContactPhone
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SetState(value);
            }
        }

        public string ImagePath
        {
            get
            {
                return GetState(SqLiteManager.DefaultEventImagePath);
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ms-appx:///"))
                {
                    SetState(SqLiteManager.DefaultEventImagePath);
                }
                else
                {
                    SetState(value);
                }
            }
        }

        public DateTime EventBegin
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
                    if (EventEnd < EventBegin)
                    {
                        EventEnd = EventBegin;
                    }
                    NotifyPropertyUpdated(nameof(StartDateOffset));
                    NotifyPropertyUpdated(nameof(StartTimeString));
                }
            }
        }

        public DateTime EventEnd
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
                    if (EventBegin > EventEnd)
                    {
                        EventBegin = EventEnd;
                    }

                    NotifyPropertyUpdated(nameof(EndDateOffset));
                    NotifyPropertyUpdated(nameof(EndTimeString));

                    if (string.IsNullOrWhiteSpace(EventTitle))
                    {
                        if (EventEnd.AddDays(1) < DateTime.Now)
                        {
                            PageTitle = Q.Resources.Event_Placeholder_SetTitle_PastEvent;
                        }
                        else
                        {
                            PageTitle = Q.Resources.Event_Placeholder_SetTitle;
                        }
                    }
                    else
                    {
                        if (EventEnd.AddDays(1) < DateTime.Now)
                        {
                            PageTitle = string.Format(Q.Resources.Event_Placeholder_Titled_PastEvent, EventTitle);
                        }
                        else
                        {
                            PageTitle = EventTitle;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Wrapper around <code>EventBegin</code>
        /// </summary>
        public DateTimeOffset? StartDateOffset
        {
            get
            {
                return new DateTimeOffset(EventBegin);
            }
            set
            {
                if (value.HasValue && SetState(value))
                {
                    var itmeSpan = AppViewModel.Settings.GetTimespan(StartTimeString);
                    EventBegin = new DateTime(
                        value.Value.Year,
                        value.Value.Month,
                        value.Value.Day,
                        EventBegin.Hour,
                        EventBegin.Minute,
                        0);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? EndDateOffset
        {
            get
            {
                return new DateTimeOffset(EventEnd);
            }
            set
            {
                if (value.HasValue && SetState(value))
                {
                    EventEnd = new DateTime(
                        value.Value.Year,
                        value.Value.Month,
                        value.Value.Day,
                        EventEnd.Hour,
                        EventEnd.Minute,
                        0);
                }
            }
        }

        [NotMapped]
        public string StartTimeString
        {
            get
            {
                return EventBegin.ToString("h:mmtt");
            }
            set
            {
                if (value != StartTimeString)
                {
                    var newTime = AppViewModel.Settings.GetTimespan(value);
                    EventBegin = new DateTime(
                         EventBegin.Year,
                         EventBegin.Month,
                         EventBegin.Day,
                         newTime.Hours,
                         newTime.Minutes,
                         0);
                }
            }
        }

        [NotMapped]
        public string EndTimeString
        {
            get
            {
                return EventEnd.ToString("h:mmtt");
            }
            set
            {
                if (value != EndTimeString)
                {
                    var newTime = AppViewModel.Settings.GetTimespan(value);
                    EventEnd = new DateTime(
                        EventEnd.Year,
                        EventEnd.Month,
                        EventEnd.Day,
                        newTime.Hours,
                        newTime.Minutes,
                        0);
                }
            }
        }

        [NotMapped]
        public ImageSource Image
        {
            get
            {
                // return GetState(new BitmapImage(new Uri(ImagePath)));
                throw new NotImplementedException();
            }

            set
            {
                // SetState(value);
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// List of guests
        /// </summary>
        [NotMapped]
        public EventGuestListModel GuestListModel
        {
            get
            {
                return GetState<EventGuestListModel>();
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Update this list with guest IDs to save.
        /// </summary>
        public HashSet<int> EditedGuestList { get; private set; }
        #endregion

        #region Manage Event
        [NotMapped]
        public bool IsNewEvent
        {
            get
            {
                return GetState(false);
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Set whether we allow user to edit event
        /// </summary>
        [NotMapped]
        public bool EditEvent
        {
            get
            {
                return GetState(false);
            }
            set
            {
                if (SetState(value))
                {
                    NotifyPropertyUpdated(nameof(CanTakeAttendance));
                }
            }
        }

        /// <summary>
        /// Edit event values
        /// </summary>
        [NotMapped]
        public RelayCommand EditEventCommand
        {
            get
            {
                return Command(() =>
                {
                    EditEvent = true;
                    EditedGuestList.Clear();

                    if (IsNewEvent)
                    {
                        // Edit existing event
                        SubMenu.Clear();
                        SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_SaveEvent, "Save", SaveEventCommand));
                        SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_DiscardChanges, "Cancel", CancelEventChangesCommand));
                    }
                    else
                    {
                        // Edit existing event
                        SubMenu.Clear();
                        SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditAttendance, "Contact2", EditAttendanceCommand));
                        SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_SaveEvent, "Save", SaveEventCommand));
                        SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_DiscardChanges, "Cancel", CancelEventChangesCommand));
                    }

                    // Make backup
                    meetingBackup = new Meeting
                    {
                        EventTitle = EventTitle,
                        Venue = Venue,
                        Description = Description,
                        EventStart = EventBegin,
                        EventEnd = EventEnd,
                        ImagePath = ImagePath,
                        VenueContactEmail = VenueContactEmail,
                        VenueContactPhone = VenueContactPhone,
                        MeetingID = ID
                    };
                });
            }
        }

        /// <summary>
        /// Used to manage event details
        /// </summary>
        public RelayCommand ViewEventCommand
        {
            get
            {
                return Command(() =>
                {
                    SetMainMenu();

                    // Edit existing event
                    SubMenu.Clear();
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditAttendance, "Contact2", EditAttendanceCommand));
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditEvent, "Edit", EditEventCommand));
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_DeleteEvent, "Delete", DeleteEventCommand));
                });
            }
        }

        [NotMapped]
        public RelayCommand SaveEventCommand
        {
            get
            {
                return Command(() =>
                {
                    if (string.IsNullOrWhiteSpace(EventTitle))
                    {
                        // Display message
                        AppViewModel.MessageDialog(
                            Q.Resources.Event_SaveConfirmDialog_Message,
                            Q.Resources.Event_SaveConfirmDialog_Title,
                            MessageDialogOptions.OK);
                    }
                    else
                    {
                        EventSaveAction();
                    }
                });
            }
        }

        /// <summary>
        /// Edit event values
        /// </summary>
        [NotMapped]
        public RelayCommand DeleteEventCommand
        {
            get
            {
                return Command(() =>
                {
                    DeleteEventAsync();
                });
            }
        }

        protected async void DeleteEventAsync()
        {
            // Display message
            AppViewModel.MessageDialog(
               Q.Resources.Event_DeleteConfirmDialog_Message,
               Q.Resources.Event_DeleteConfirmDialog_Title,
                MessageDialogOptions.Okay_Cancel);

            await Task.Factory.StartNew(() =>
            {
                while (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.No_Results)
                    Task.Delay(100);
            });

            // Wants to cancel action
            if (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.Okay)
            {
                AppViewModel.Calendar.DeleteEvent(this);

                AppViewModel.GoTo(typeof(CalendarMonthPage), AppViewModel.Calendar);
            }
        }

        [NotMapped]
        public RelayCommand CancelEventChangesCommand
        {
            get
            {
                return Command(CancelChangesCommand);
            }
        }

        private async void CancelChangesCommand()
        {
            if (await CancelChangesCommandTask())
            {
                // Restore menu
                ViewEventCommand.Execute();
            }
        }

        /// <summary>
        /// Confirm if user wants to cancel changes
        /// </summary>
        private async Task<bool> CancelChangesCommandTask()
        {
            if (//AttendanceListModified ||
                !meetingBackup.EventTitle.Equals(EventTitle) ||
                !meetingBackup.Venue.Equals(Venue) ||
                !meetingBackup.Description.Equals(Description) ||
                !meetingBackup.VenueContactEmail.Equals(VenueContactEmail) ||
                !meetingBackup.VenueContactPhone.Equals(VenueContactPhone) ||
                !meetingBackup.ImagePath.Equals(ImagePath) ||

                !(meetingBackup.EventStart.Year == EventBegin.Year &&
                meetingBackup.EventStart.Month == EventBegin.Month &&
                meetingBackup.EventStart.Day == EventBegin.Day) ||

                !(meetingBackup.EventEnd.Year == EventEnd.Year &&
                meetingBackup.EventEnd.Month == EventEnd.Month &&
                meetingBackup.EventEnd.Day == EventEnd.Day))
            {
                // Verify user wants to discard changes
                AppViewModel.MessageDialogResults = MessageDialogResultsEnum.No_Results;
                AppViewModel.MessageDialog(
                     Q.Resources.Event_CancelConfirmDialog_Message,
                     Q.Resources.Event_CancelConfirmDialog_Title,
                     MessageDialogOptions.Okay_Cancel);

                await Task.Factory.StartNew(() =>
                {
                    while (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.No_Results)
                        Task.Delay(100);
                });

                // Wants to cancel action
                if (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.Okay)
                {
                    return EventCancelAction();
                }
                else
                {
                    // Return to editing
                    return false;
                }
            }
            else
            {
                return EventCancelAction();
            }
        }

        /// <summary>
        /// Used to perform any custom cancel action
        /// </summary>
        protected abstract bool EventCancelAction();

        /// <summary>
        /// Used to perform any custom save action
        /// </summary>
        /// <returns></returns>
        protected abstract void EventSaveAction();

        protected Meeting meetingBackup;
        #endregion

        #region Manage Attendance
        [NotMapped]
        public bool CanTakeAttendance
        {
            get
            {
                return DateTime.Now >= EventBegin &&
                    GuestListModel.GuestList.Items.Count > 0 &&
                    !EditEvent;
            }
        }

        /// <summary>
        /// Set whether we allow user to edit Attendance
        /// </summary>
        [NotMapped]
        public bool EditAttendance
        {
            get
            {
                return GetState(false);
            }
            set
            {
                SetState(value);
            }
        }

        [NotMapped]
        public bool AttendanceListModified
        {
            get
            {
                return GetState(false);
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Set to true if attendance has been updated
        /// </summary>
        public bool AttendanceUpdated { get; set; }

        public RelayCommand EditAttendanceCommand
        {
            get
            {
                return Command(() =>
                {
                    MainMenu = new ObservableCollection<MenuItem<AppLevelModel>>
                    {
                        new MenuItem<AppLevelModel>(Q.Resources.Event_ReturnToCurrentEvent, "ImportAll", AcceptAttendanceCommand)
                    };

                    EditAttendance = true;
                    SubMenu.Clear();
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_AcceptChanges, "Accept", AcceptAttendanceCommand));
                    AppViewModel.GoTo(typeof(AddRemoveEventGuestsPage), this, null, true);

                    AttendanceListModified = true;
                });
            }
        }

        public RelayCommand AcceptAttendanceCommand
        {
            get
            {
                return Command(() =>
                {
                    AppViewModel.GoBack();
                });
            }
        }
        #endregion

        #region Navigation
        public async override Task<bool> OnNavigatingFrom()
        {
            if (EditAttendance)
            {
                // We are finished editing attendance
                EditAttendance = false;

                if (AttendanceUpdated)
                {
                    var cancelResult = await CancelChangesCommandTask();

                    AttendanceUpdated = cancelResult ? false : true;
                    SetMainMenu();

                    return cancelResult;
                }
                else
                {
                    SetMainMenu();

                    return true;
                }
            }
            else
            {
                if (EditEvent)
                {
                    var cancelResult = await CancelChangesCommandTask();
                    EditEvent = cancelResult ? false : true;

                    if (IsNewEvent && cancelResult)
                    {
                        //   SqLiteManager.DeleteEventAttendance(ID);

                        //   GuestListModel.GuestList.Clear();
                    }

                    SetMainMenu();

                    return cancelResult;
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion

        #region Rest
        //private ObservableCollection<MenuItem<AppLevelModel>> oldSubmenu;
        //private int oldSubmenuIndex;

        /// <summary>
        /// Not used
        /// </summary>
        public override string SearchTerm
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SearchTermChanged?.Invoke(this, value);
                SetState(value);
            }
        }
        #endregion

        #region Mapping
        [NotMapped]
        public Geopoint MapCenter
        {
            get
            {
                return GetState<Geopoint>();
            }
            set
            {
                SetState(value);
            }
        }

        private async void SetMapCenter()
        {
            try
            {
                MapLocationFinderResult result =
                    await MapLocationFinder.FindLocationsAsync("13121 129th Ct NE, Kirkland, WA 98034", null);

                MapCenter = result.Locations.FirstOrDefault().Point;
            }
            catch
            {
            }
        }
        #endregion
    }

    public delegate void SearchTermChangedHandler(object sender, string searchTerm);
}
