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
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddRemoveEventGuestsPage : Page
    {
        private MyContactsModel allContacts { get; set; }
        private BaseEventItemModel theEvent { get; set; }

        public AddRemoveEventGuestsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            theEvent = e?.Parameter as BaseEventItemModel;
            allContacts = theEvent.AppViewModel.AllContacts;

            if (allContacts.Contacts.Items.Count == 0)
            {
                NavigateToNewContactsListPanel.Visibility = Visibility.Visible;
                zoom.Visibility = Visibility.Collapsed;
                AcceptChangesPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                NavigateToNewContactsListPanel.Visibility = Visibility.Collapsed;
                zoom.Visibility = Visibility.Visible;
                AcceptChangesPanel.Visibility = Visibility.Visible;

                theEvent.ImplimentsSearch = true;

                theEvent.SearchTermChanged += (sender, searchTerm) =>
                {
                    allContacts.Contacts.SearchTerm = searchTerm;
                };

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

                foreach (var contact in allContacts.Contacts.Items)
                {
                    if (theEvent.GuestListModel.GuestList.ContainsContact(contact.ID))
                    {
                        GuestGridViewIn.SelectedItems.Add(contact);
                    }
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            //foreach (var item in GuestGridViewIn.SelectedItems)
            //{
            //    theEvent.EditedGuestList.Add((item as ContactItemModel).ID);
            //}

            foreach (var contact in allContacts.Contacts.Items)
            {
                if (GuestGridViewIn.SelectedItems.Contains(contact))
                {
                    // Add contact if not laready there
                    if (!theEvent.GuestListModel.GuestList.ContainsContact(contact.ID))
                    {
                        var newEvent = SqLiteManager.AddGuestToEvent(theEvent.ID, contact);
                        theEvent.GuestListModel.GuestList.Add(newEvent);
                    }
                }
                else
                {
                    var eventGuest = theEvent.GuestListModel.GuestList.GetEventGuest(contact.ID);
                    if (eventGuest != null)
                    {
                        SqLiteManager.RemoveGuestFromEvent(eventGuest.ID);

                        // Remove contact
                        theEvent.GuestListModel.GuestList.RemoveContact(contact.ID);
                    }
                }
            }
        }
    }
}
