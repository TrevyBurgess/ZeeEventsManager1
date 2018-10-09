//------------------------------------------------------------
// <copyright file="ContactItemPage.xaml" company="TrevyBurgess" >
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactItemPage : Page
    {
        public ContactItemPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ContactModel = e?.Parameter as BaseContactItemModel;

            if (ContactModel.EditContact)
            {
                ContactModel.EditCommand.Execute();
                EditButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                SaveCancelPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                EditButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
                SaveCancelPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            ContactModel.AppViewModel.AllContacts.ImplimentsSearch = ContactModel.TiledWithAllContactsPage;
        }

        internal BaseContactItemModel ContactModel { get; private set; }

        public void LoseFocus()
        {
            contactItemPage.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }

        private void contactItemPage_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (ActualWidth > 600)
            {
                scrollPanel.Width = 600;
            }
            else
            {
                scrollPanel.Width = ActualWidth;
            }
        }

        #region Contact Values
        private void FirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ContactModel.FirstName = FirstName.Text;
        }

        private void LastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ContactModel.LastName = LastName.Text;
        }

        private void PhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ContactModel.PhoneNumber = PhoneNumber.Text;
        }

        private void PhoneNumber2_TextChanged(object sender, TextChangedEventArgs e)
        {
            ContactModel.PhoneNumber2 = PhoneNumber2.Text;
        }

        private void EmailAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            ContactModel.EmailAddress = EmailAddress.Text;
        }

        private void EmailAddress2_TextChanged(object sender, TextChangedEventArgs e)
        {
            ContactModel.EmailAddress2 = EmailAddress2.Text;
        }

        private void AvatarSelectorFlyout_Opened(object sender, object e)
        {
            var selectorGrid = AvatarSelectorFlyout.Content as GridView;
            if (ActualWidth < 500)
                selectorGrid.Width = 300;
            else
                selectorGrid.Width = 400;

            selectorGrid.Height = ActualHeight;
        }

        private void AvatarSelectorGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            ContactModel.ImagePath = e.ClickedItem as string;
            AvatarImageButton.Flyout.Hide();
        }
        #endregion

        private void EditButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ContactModel.EditCommand.Execute();
            EditButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            SaveCancelPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void CancelButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ContactModel.CancelChangesCommand.Execute();
            EditButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            SaveCancelPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void SaveButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ContactModel.SaveChangesCommand.Execute();
            EditButton.Visibility = Windows.UI.Xaml.Visibility.Visible;
            SaveCancelPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
