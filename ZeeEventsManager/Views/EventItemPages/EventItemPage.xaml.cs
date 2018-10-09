//------------------------------------------------------------
// <copyright file="EventItemPage.xaml" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Windows.UI.Xaml.Media;

    public sealed partial class EventItemPage : Page
    {
        internal BaseEventItemModel EventsModel { get; private set; }

        public EventItemPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            EventsModel = e?.Parameter as BaseEventItemModel;
            if (EventsModel.EditEvent)
            {
                editButton.Visibility = Visibility.Collapsed;
                SaveCancelPanel.Visibility = Visibility.Visible;
                EventsModel.EditEventCommand.Execute();
            }
            else if (!EventsModel.IsNewEvent)
            {
                editButton.Visibility = Visibility.Visible;
                SaveCancelPanel.Visibility = Visibility.Collapsed;
            }

            eventGuestFrame.Navigate(typeof(EventGuestListPage), EventsModel.GuestListModel);
        }

        private void VenueSelectorGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            EventsModel.ImagePath = e.ClickedItem as string;
            VenueImageButton.Flyout.Hide();
        }

        private void AvatarSelectorFlyout_Opened(object sender, object e)
        {
            //var selectorGrid = AvatarSelectorFlyout.Content as GridView;
            //if (ActualWidth < 500)
            //    selectorGrid.Width = 300;
            //else
            //    selectorGrid.Width = 400;

            //selectorGrid.Height = ActualHeight;
        }

        private void AttendanceButton_Click(object sender, RoutedEventArgs e)
        {
            EventsModel.AppViewModel.GoTo(
                typeof(EventGuestAttendancePage),
                EventsModel.GuestListModel);
        }

        private void MainEventPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainEventPage.ActualHeight > 40)
                scrollviewer.Height = MainEventPage.ActualHeight - 40;
            else
                scrollviewer.Height = MainEventPage.ActualHeight;
            
            EventsModel.ImplimentsSearch = GuestPanel.Visibility == Visibility.Visible;
        }

        private ImageSource TakeAttendance = SettingsModel.TakeAttendance;
        private ImageSource EditAttendance = SettingsModel.EditAttendance;

        #region Data changed
        private void EventTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            EventsModel.EventTitle = EventTitle.Text;
        }

        private void Venue_TextChanged(object sender, TextChangedEventArgs e)
        {
            EventsModel.Venue = Venue.Text;
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            EventsModel.Description = Description.Text;
        }

        private void VenueContactEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            EventsModel.VenueContactEmail = VenueContactEmail.Text;
        }

        private void VenueContactPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            EventsModel.VenueContactPhone = VenueContactPhone.Text;
        }
        #endregion

        private void EditAttendanceButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            EventsModel.EditAttendanceCommand.Execute();
            editAttendanceButton.Visibility = Visibility.Collapsed;
            SaveCancelPanel.Visibility = Visibility.Visible;
        }

        private void EditButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            editButton.Visibility = Visibility.Collapsed;
            SaveCancelPanel.Visibility = Visibility.Visible;
            EventsModel.EditEventCommand.Execute();
        }

        private void CancelButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            editButton.Visibility = Visibility.Visible;
            SaveCancelPanel.Visibility = Visibility.Collapsed;
            EventsModel.CancelEventChangesCommand.Execute();
        }

        private void SaveButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            editButton.Visibility = Visibility.Visible;
            SaveCancelPanel.Visibility = Visibility.Collapsed;
            EventsModel.SaveEventCommand.Execute();
        }
    }
}
