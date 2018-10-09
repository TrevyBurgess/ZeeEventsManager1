//------------------------------------------------------------
// <copyright file="AllContactsPage.xaml" company="TrevyBurgess" >
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
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyContactsPage : Page
    {
        public MyContactsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            allContacts = e?.Parameter as MyContactsModel;

            allContacts.ImplimentsSearch = true;

            allContacts.Contacts.CollectionChanged += (s, e2) =>
            {
                zoom.InvalidateArrange();
                contactSource.Source = allContacts.Contacts.ItemsByCategory;
                (zoom.ZoomedOutView as ListViewBase).ItemsSource =
                contactSource.View.CollectionGroups;
            };

            contactSource.Source = allContacts.Contacts.ItemsByCategory;
            (zoom.ZoomedOutView as ListViewBase).ItemsSource =
                contactSource.View.CollectionGroups;

            if (allContacts.Contacts.Items.Count > 0)
            {
                currentModel = GuestGridViewIn.Items[0] as ContactItemModel;
                currentModel.TiledWithAllContactsPage = true;
                contactFrame.Navigate(typeof(ContactItemPage), currentModel);
            }
        }

        private MyContactsModel allContacts { get; set; }

        private void GuestGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            currentModel = e.ClickedItem as ContactItemModel;

            if (ActualWidth < (double)Application.Current.Resources["AppDoubleWidth"])
            {
                allContacts.AppViewModel.GoTo(typeof(ContactItemPage), currentModel);
                currentModel.TiledWithAllContactsPage = false;
            }
            else
            {
                currentModel.TiledWithAllContactsPage = true;
            }

            contactFrame.Navigate(typeof(ContactItemPage), currentModel);
        }

        private ContactItemModel currentModel;

        private void allContactsPage1_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (ActualWidth < 700)
            {
                zoom.Width = ActualWidth - 40;
            }
            else
            {
                zoom.Width = 700;
            }

            if (allContacts.Contacts.Items.Count == 0)
            {
                addContactButtonPanel.Visibility = Visibility.Visible;
                addContactButtonPanel.Margin = new Thickness(50);
                addContactMessage.Visibility = Visibility.Visible;
                MainEventGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainEventGrid.Visibility = Visibility.Visible;
                addContactMessage.Visibility = Visibility.Collapsed;
                addContactButtonPanel.Margin = new Thickness(0);

                if (ActualWidth < (double)Application.Current.Resources["AppNarrowWidth"])
                {
                    addContactButtonPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    addContactButtonPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void contactPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (contactPanel.ActualWidth < 700)
            {
                contactFrame.Width = contactPanel.ActualWidth;
            }
            else
            {
                contactFrame.Width = 700;
            }
        }

        private void contactFrame_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            editButton.Margin = new Thickness((contactFrame.ActualWidth / 2) - 80, 5, 5, 5);
            acceptCancelPanel.Margin = new Thickness((contactFrame.ActualWidth / 2) - 200, 5, 5, 5);
        }

        private void editButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            currentModel.EditCommand.Execute();
            currentModel.TiledWithAllContactsPage = true;

            editButton.Visibility = Visibility.Collapsed;
            acceptCancelPanel.Visibility = Visibility.Visible;
        }

        private void acceptButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            currentModel.SaveChangesCommand.Execute();
            currentModel.TiledWithAllContactsPage = true;

            editButton.Visibility = Visibility.Visible;
            acceptCancelPanel.Visibility = Visibility.Collapsed;

            // Update UI
            zoom.InvalidateArrange();
            contactSource.Source = allContacts.Contacts.ItemsByCategory;
            (zoom.ZoomedOutView as ListViewBase).ItemsSource =
            contactSource.View.CollectionGroups;
        }

        private void cancelButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            currentModel.CancelChangesCommand.Execute();
            currentModel.TiledWithAllContactsPage = false;

            editButton.Visibility = Visibility.Visible;
            acceptCancelPanel.Visibility = Visibility.Collapsed;
        }

        private void mainGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            allContacts.AddContactCommand.Execute();
        }

        private void AddContactButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            allContacts.AddContactCommand.Execute();
        }
    }
}
