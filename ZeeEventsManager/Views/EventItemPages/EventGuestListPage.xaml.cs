//------------------------------------------------------------
// <copyright file="EventGuestListPage.xaml" company="TrevyBurgess" >
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
    using System;

    /// <summary>
    /// Used to add/remove guests
    /// </summary>
    public sealed partial class EventGuestListPage : Page
    {
        public EventGuestListPage()
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
            var model = (e.ClickedItem as EventGuest).Guest;
            guestList.AppViewModel.GoTo(typeof(ContactItemPage), model);
        }

        private void GuestPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}
