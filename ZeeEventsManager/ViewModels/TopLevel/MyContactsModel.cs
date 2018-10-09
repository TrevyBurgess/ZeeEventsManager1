//------------------------------------------------------------
// <copyright file="MyContactsModel.cs" company="TrevyBurgess" >
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
    using System;

    public class MyContactsModel : PageViewmodelBase<AppLevelModel>
    {
        /// <summary>
        /// Get all contacts
        /// </summary>
        public MyContactsModel(AppLevelModel appViewModel) : base(appViewModel)
        {
            IconCode = "People";
            PageTitle = Q.Resources.MyContacts_PageTitle;
            MainMenu = appViewModel.TopLevelMenu;
            menuToRestore = appViewModel.TopLevelMenu;
            MainMenuIndex = 2;

            addContactsMenu = new ObservableCollection<MenuItem<AppLevelModel>>();
            addContactsMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.MyContacts_BackToEventList, "ImportAll"));

            SqLiteManager.GetAllContacts(appViewModel, Contacts);

            SubMenu.Add(new MenuItem<AppLevelModel>(
                    appViewModel.NewContact,
                    typeof(ContactItemPage), ResetNewContact));
            SubMenuIndex = -1;
            ResetSubMenuIndexOnNavigation = true;

            BottomMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(
                    appViewModel.Settings.AboutViewModel, typeof(AboutPage) )
            };
            BottomMenuIndex = -1;
            ResetBottomMenuIndexOnNavigation = true;

            ImplimentsSearch = true;
        }

        /// <summary>
        /// Used to get new contacts from events
        /// </summary>
        public RelayCommand NavigatedMainMenu
        {
            get
            {
                return Command(() =>
                {
                    menuToRestore = AppViewModel.TopLevelMenu;
                    MainMenu = AppViewModel.TopLevelMenu;
                    MainMenuIndex = 2;
                });
            }
        }

        /// <summary>
        /// Used to get new contacts from events
        /// </summary>
        public RelayCommand NavigatedFromEvent
        {
            get
            {
                return Command(() =>
                {
                    menuToRestore = addContactsMenu;
                    MainMenu = addContactsMenu;
                    MainMenuIndex = -1;

                    AppViewModel.GoTo(typeof(MyContactsPage), this, null, true);
                });
            }
        }
        
        public override string SearchTerm
        {
            get
            {
                return Contacts.SearchTerm;
            }
            set
            {
                Contacts.SearchTerm = value;
            }
        }

        public ContactCollectionModel Contacts
        {
            get
            {
                return GetState(new ContactCollectionModel());
            }
            set
            {
                SetState(value);
            }
        }

        public RelayCommand ResetNewContact
        {
            get
            {
                return Command(() =>
                {
                    // Get new blank contact
                    AppViewModel.NewContact.EmailAddress = string.Empty;
                    AppViewModel.NewContact.EmailAddress2 = string.Empty;
                    AppViewModel.NewContact.FirstName = string.Empty;
                    AppViewModel.NewContact.LastName = string.Empty;
                    AppViewModel.NewContact.PhoneNumber = string.Empty;
                    AppViewModel.NewContact.PhoneNumber2 = string.Empty;
                    AppViewModel.NewContact.ResetSubMenuIndexOnNavigation = true;
                    AppViewModel.NewContact.ImagePath = SqLiteManager.DefaultContactImagePath;
                    AppViewModel.NewContact.EditCommand.Execute();
                });
            }
        }

        public RelayCommand AddContactCommand
        {
            get
            {
                return Command(() =>
                {
                    AppViewModel.NewContact.EditContact = true;
                    AppViewModel.GoTo(typeof(ContactItemPage), AppViewModel.NewContact, ResetNewContact);
                });
            }
        }

        /// <summary>
        /// Add contact, and save to file
        /// </summary>
        internal void AddContact(NewContactItemModel newContactItemModel)
        {
            Contacts.AddOrUpdate(newContactItemModel, AppViewModel.Settings.UserNameInStandardFormat.Value);
        }

        public override Task<bool> OnNavigatingFrom()
        {
            MainMenu = menuToRestore;

            return Task.Run(() => { return true; });
        }

        private ObservableCollection<MenuItem<AppLevelModel>> addContactsMenu;

        private ObservableCollection<MenuItem<AppLevelModel>> menuToRestore;
    }
}
