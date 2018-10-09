//------------------------------------------------------------
// <copyright file="AppLevelModel.cs" company="TrevyBurgess" >
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

    public class AppLevelModel : AppLevelViewModelBase<AppLevelModel>
    {
        private static AppLevelModel viewModel;

        private AppLevelModel()
        {
            FeedbackEmail = "ZeeEventsManager@CyberFeedForward.com";
            FeedbackAppName = "ZeeEventsManager";

            GuestMenuForAllContacts =
            new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(Q.Resources.AppLevel_MyContacts, "ImportAll")
            };

            GuestMenuForEventContacts =
            new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(Q.Resources.AppLevel_CurrentEvent, "ImportAll")
            };

            Settings = new SettingsModel(this);

            NewContact = new NewContactItemModel(this);
            AllContacts = new MyContactsModel(this);
            Calendar = new CalendarModel(this);
            Calendar.CurrentDate = DateTime.Now;

            // Dynamically update viewmodel as necessary
            latestEventMenuItem = new MenuItem<AppLevelModel>(
                null,
                typeof(EventItemPage),
                null,
                GetLatestEventSetup,
                Q.Resources.AppLevel_LatestEvent,
                "GoToToday");

            // Define top level menu, in required order
            TopLevelMenu.Add(new MenuItem<AppLevelModel>(Calendar, typeof(CalendarMonthPage)));
            TopLevelMenu.Add(latestEventMenuItem);
            TopLevelMenu.Add(new MenuItem<AppLevelModel>(AllContacts, typeof(MyContactsPage), null, AllContacts.NavigatedMainMenu));
            TopLevelMenu.Add(new MenuItem<AppLevelModel>(Settings, typeof(SettingsPage)));

            // Set current date
            Calendar.CurrentDate = DateTime.Now;
        }

        public static AppLevelModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new AppLevelModel();
                }

                return viewModel;
            }
        }

        public CalendarModel Calendar { get; }

        private RelayCommand GetLatestEventSetup
        {
            get
            {
                return Command(() =>
                {
                    latestEventMenuItem.ViewModel = SqLiteManager.GetLatestEvent(this);
                    if (latestEventMenuItem.ViewModel == null)
                    {
                        latestEventMenuItem.ViewModel = NewEventItemModel.GetNewEvent(DateTime.Now, false);
                    }

                    var theEvent = latestEventMenuItem.ViewModel as BaseEventItemModel;
                    if (theEvent.ID == NewEventId)
                    {
                        theEvent.EditEvent = true;
                        theEvent.IsNewEvent = true;
                        theEvent.EditEventCommand.Execute();
                        latestEventMenuItem.ViewModel.PageTitle = Q.Resources.AppLevel_CreateNewEvent;
                    }
                    else
                    {
                        theEvent.EditEvent = false;
                        theEvent.IsNewEvent = false;
                    }
                });
            }
        }

        public MyContactsModel AllContacts { get; }

        public SettingsModel Settings { get; }

        public ObservableCollection<MenuItem<AppLevelModel>> GuestMenuForAllContacts { get; }

        public ObservableCollection<MenuItem<AppLevelModel>> GuestMenuForEventContacts { get; }

        private MenuItem<AppLevelModel> latestEventMenuItem { get; }

        /// <summary>
        /// Used when creating a new contact. Will be reset when contact is saved (is no longer new)
        /// </summary>
        public NewContactItemModel NewContact { get; private set; }

        internal void ResetNewContact()
        {
            NewContact.ID = -1;
            NewContact.EmailAddress = string.Empty;
            NewContact.EmailAddress2 = string.Empty;
            NewContact.FirstName = string.Empty;
            NewContact.LastName = string.Empty;
            NewContact.PhoneNumber = string.Empty;
            NewContact.PhoneNumber2 = string.Empty;
            NewContact.ImagePath = SqLiteManager.DefaultContactImagePath;
            NewContact.Category = null;
        }

        /// <summary>
        /// If -1, no new entry in database exists.
        /// Else, new entry with this ID exists.
        /// </summary>
        public int NewEventId
        {
            get
            {
                return GetState(-1, SaveType.RoamingSettings);
            }

            set
            {
                SetState(value, SaveType.RoamingSettings);
            }
        }
    }
}