//------------------------------------------------------------
// <copyright file="ContactItemModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Threading.Tasks;

    public class ContactItemModel : BaseContactItemModel
    {
        public ContactItemModel(AppLevelModel appViewModel) : base(appViewModel)
        {
            // Used for Navigation
            IconCode = "AddFriend";
            PageTitle = Q.Resources.ContactItem_PageTitle;

            MainMenu = AppViewModel.GuestMenuForAllContacts;
            MainMenuIndex = 0;

            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Edit, "Edit", EditCommand));
            SubMenuIndex = -1;
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

                    backupContact = new Contact
                    {
                        ContactID = ID,
                        EmailAddress = EmailAddress,
                        EmailAddress2 = EmailAddress2,
                        FirstName = FirstName,
                        LastName = LastName,
                        PhoneNumber = PhoneNumber,
                        PhoneNumber2 = PhoneNumber2,
                        ImagePath = ImagePath
                    };
                });
            }
        }

        #region Cancel changes
        [NotMapped]
        public override RelayCommand SaveChangesCommand
        {
            get
            {
                return Command(() =>
                {
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
                        FirstName = FirstName.Trim();
                        LastName = LastName.Trim();

                        SubMenu.Clear();
                        SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Edit, "Edit", EditCommand));
                        EditContact = false;

                        // Set category
                        if (Category == null)
                        {
                            var catName = AppViewModel.Settings.UserNameInStandardFormat.Value ?
                            LastName[0].ToString().ToUpper() :
                            FirstName[0].ToString().ToUpper();
                            Category = GetCategory(catName);
                        }

                        // Update contact
                        SqLiteManager.AddOrUpdateContact(this);
                    }
                });
            }
        }

        [NotMapped]
        public override RelayCommand CancelChangesCommand
        {
            get
            {
                return Command(For_CancelChangesCommand);
            }
        }

        private async void For_CancelChangesCommand()
        {
            if (await CancelChangesCommandTask())
            {
                EditContact = false;
                SubMenu.Clear();
                SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Edit, "Edit", EditCommand));
                SubMenuIndex = -1;
            }
        }

        private async Task<bool> CancelChangesCommandTask()
        {
            if (!backupContact.EmailAddress.Equals(EmailAddress) ||
                !backupContact.EmailAddress.Equals(EmailAddress) ||
                !backupContact.EmailAddress2.Equals(EmailAddress2) ||
                !backupContact.FirstName.Equals(FirstName) ||
                !backupContact.ImagePath.Equals(ImagePath) ||
                !backupContact.LastName.Equals(LastName) ||
                !backupContact.PhoneNumber.Equals(PhoneNumber) ||
                !backupContact.PhoneNumber2.Equals(PhoneNumber2) ||
                !backupContact.ImagePath.Equals(ImagePath))
            {
                AppViewModel.MessageDialogResults = MessageDialogResultsEnum.No_Results;

                // Send message
                AppViewModel.MessageDialog(
                    Q.Resources.Contact_CancelConfirmDialog_Message,
                   Q.Resources.Contact_CancelConfirmDialog_Title,
                     MessageDialogOptions.Okay_Cancel);

                await Task.Factory.StartNew(() =>
                {
                    while (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.No_Results)
                        Task.Delay(100);
                });

                if (AppViewModel.MessageDialogResults == MessageDialogResultsEnum.Okay)
                {
                    // Cancel changes
                    SubMenu.Clear();
                    SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Contact_Edit, "Edit", EditCommand));
                    EditContact = false;

                    // Restore from backup
                    ID = backupContact.ContactID;
                    EmailAddress = backupContact.EmailAddress;
                    EmailAddress2 = backupContact.EmailAddress2;
                    FirstName = backupContact.FirstName;
                    LastName = backupContact.LastName;
                    PhoneNumber = backupContact.PhoneNumber;
                    PhoneNumber2 = backupContact.PhoneNumber;
                    ImagePath = backupContact.ImagePath;

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
        #endregion

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

        private Contact backupContact;
    }
}
