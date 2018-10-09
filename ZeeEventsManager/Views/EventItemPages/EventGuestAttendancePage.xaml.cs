//------------------------------------------------------------
// <copyright file="EventGuestAttendancePage.xaml" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Used to take attendance
    /// </summary>
    public sealed partial class EventGuestAttendancePage : Page
    {
        public EventGuestAttendancePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            guestList = e?.Parameter as EventGuestListModel;
            guestList.ImplimentsSearch = true;

            guestList.GuestList.CollectionChanged += (s, e2) =>
            {
                zoom.InvalidateArrange();
                contactSource.Source = guestList.GuestList.ItemsByCategory;
                (zoom.ZoomedOutView as ListViewBase).ItemsSource =
                contactSource.View.CollectionGroups;
            };

            contactSource.Source = guestList.GuestList.ItemsByCategory;
            (zoom.ZoomedOutView as ListViewBase).ItemsSource =
                contactSource.View.CollectionGroups;
        }

        private EventGuestListModel guestList { get; set; }

        private void GuestGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var model = e.ClickedItem as EventGuest;

            if (model.GuestStatus == GustArrivalStatus.Unknown)
                model.GuestStatus = GustArrivalStatus.Arrived;
            else if (model.GuestStatus == GustArrivalStatus.Arrived)
                model.GuestStatus = GustArrivalStatus.ArrivedLate;
            else if (model.GuestStatus == GustArrivalStatus.ArrivedLate)
                model.GuestStatus = GustArrivalStatus.DidNotCome;
            else
                model.GuestStatus = GustArrivalStatus.Unknown;

            SqLiteManager.UpdateAttendance(model);
        }
    }
}
