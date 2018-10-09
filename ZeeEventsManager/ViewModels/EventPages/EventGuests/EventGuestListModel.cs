//------------------------------------------------------------
// <copyright file="EventGuestListModel.cs" company="TrevyBurgess" >
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
    using System.Threading.Tasks;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Manage attendance here
    /// </summary>
    public class EventGuestListModel : PageViewmodelBase<AppLevelModel>
    {
        /// <summary>
        /// Get contacts for event
        /// </summary>
        public EventGuestListModel(
            AppLevelModel appViewModel,
            BaseEventItemModel eventModel) : base(appViewModel)
        {
            IconCode = "People";
            PageTitle = Q.Resources.EventAttendance_PageTitle;
            MainMenu = appViewModel.GuestMenuForEventContacts;
            MainMenuIndex = 0;

            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_AcceptChanges, "Accept", AcceptAttendanceChanges));
            SubMenuIndex = -1;
            ResetSubMenuIndexOnNavigation = true;

            // Set bottom menu
            BottomMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(
                    appViewModel.Settings.AboutViewModel, typeof(AboutPage))
            };
            BottomMenuIndex = -1;
            ResetBottomMenuIndexOnNavigation = true;

            ImplimentsSearch = true;
        }

        [NotMapped]
        public RelayCommand AcceptAttendanceChanges
        {
            get
            {
                return Command(() =>
                {
                    AppViewModel.GoBack();
                });
            }
        }

        public override string SearchTerm
        {
            get
            {
                return GuestList.SearchTerm;
            }
            set
            {
                GuestList.SearchTerm = value;
            }
        }

        public EventGuestCollectionModel GuestList
        {
            get
            {
                return GetState(new EventGuestCollectionModel());
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Set whether we want to take attendance
        /// </summary>
        [NotMapped]
        public bool TakeAttendance
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

        public override Task<bool> OnNavigatingFrom()
        {
            return Task.Run(() => { return true; });
        }
    }
}
