//------------------------------------------------------------
// <copyright file="NewContactItemModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//-----------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Threading.Tasks;

    public class NewContactItemModel : BaseContactItemModel
    {
        public NewContactItemModel(AppLevelModel appViewModel) : base(appViewModel)
        {
            // Used for Navigation
            IconCode = "AddFriend";
            PageTitle = Q.Resources.ContactNew_PageTitle;

            MainMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(Q.Resources.Contact_ReturnToMyContacts, "ImportAll")
            };
            MainMenuIndex = 0;

            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Edit, "Edit", EditCommand));
            BottomMenuIndex = -1;
            ResetSubMenuIndexOnNavigation = true;
        }

        [NotMapped]
        public override RelayCommand EditCommand
        {
            get
            {
                return Command(() =>
                {
                    EditContact = true;
                    SubMenu.Clear();
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Save, "Save", SaveChangesCommand));
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Discard, "Cancel", CancelChangesCommand));
                });
            }
        }

        [NotMapped]
        public override RelayCommand SaveChangesCommand
        {
            get
            {
                return Command(() =>
                {
                    FirstName = FirstName.Trim();
                    LastName = LastName.Trim();
                    if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                    {
                        // Display message
                        AppViewModel.MessageDialog(
                            Q.Resources.Contact_SaveConfirmDialog_Message,
                            Q.Resources.Contact_SaveConfirmDialog_Title,
                            MessageDialogOptions.OK);
                    }
                    else
                    {

                        SubMenu.Clear();
                        SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Edit, "Edit", EditCommand));
                        EditContact = false;

                        // Add to contact list, and save to file
                        AppViewModel.AllContacts.AddContact(this);

                        AppViewModel.ResetNewContact();
                        AppViewModel.GoBack();
                    }
                });
            }
        }

        [NotMapped]
        public override RelayCommand CancelChangesCommand
        {
            get
            {
                return Command(CancelChangesAsync);
            }
        }

        /// <summary>
        /// Perform any maintenance tasks needed before navigating away from page
        /// </summary>
        /// <returns>True if navigating back is allowed, false otherwise</returns>
        public async override Task<bool> OnNavigatingFrom()
        {
            // We haven't saved
            if (EditContact)
            {
                var res = await CancelChangesCommandTask();

                EditContact = res ? false : true;

                return res;
            }
            else
            {
                return true;
            }
        }

        private async void CancelChangesAsync()
        {
            if (await CancelChangesCommandTask())
                AppViewModel.GoBack();
        }

        /// <summary>
        /// Confirm if user wants to cancel changes
        /// </summary>
        private async Task<bool> CancelChangesCommandTask()
        {
            if (!string.IsNullOrWhiteSpace(EmailAddress) ||
                !string.IsNullOrWhiteSpace(EmailAddress2) ||
                !string.IsNullOrWhiteSpace(FirstName) ||
                !string.IsNullOrWhiteSpace(LastName) ||
                !string.IsNullOrWhiteSpace(PhoneNumber) ||
                !string.IsNullOrWhiteSpace(PhoneNumber2))
            {
                AppViewModel.MessageDialogResults = MessageDialogResultsEnum.No_Results;

                // Send message
                AppViewModel.MessageDialog(
                     Q.Resources.Contact_SaveConfirmDialog_Message,
                     Q.Resources.Contact_CancelConfirmDialog_Title,
                     MessageDialogOptions.Okay_Cancel);

                await Task.Factory.StartNew(() =>
                {
                    while (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.No_Results)
                        Task.Delay(100);
                });

                if (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.Okay)
                {
                    SubMenu.Clear();
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Edit, "Edit", EditCommand));
                    EditContact = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
