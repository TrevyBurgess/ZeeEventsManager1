//------------------------------------------------------------
// <copyright file="BaseContactItemModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    public abstract class BaseContactItemModel : PageViewmodelBase<AppLevelModel>,
        IItemModel<ContactCategoryModel>
    {
        public BaseContactItemModel(AppLevelModel appViewModel) : base(appViewModel)
        {
            BottomMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(appViewModel.Settings.AboutViewModel, typeof(AboutPage) )
            };
        }

        #region Contact values
        public int ID
        {
            get
            {
                return GetState(-1);
            }
            set
            {
                SetState(value);
            }
        }

        public string FirstName
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                if (SetState(value))
                {
                    UpdatePageTitle();
                }
            }
        }

        public string LastName
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                if (SetState(value))
                {
                    UpdatePageTitle();
                }
            }
        }

        private void UpdatePageTitle()
        {
            var firstName = string.IsNullOrWhiteSpace(FirstName) ? Q.Resources.Contact_Placeholder_FirstName : FirstName;
            var lastName = string.IsNullOrWhiteSpace(LastName) ? Q.Resources.Contact_Placeholder_LastName : LastName;

            PageTitle = AppViewModel.Settings.UserNameInStandardFormat.Value ?
                $"{lastName}, {firstName}" : $"{firstName} {lastName}";
        }

        public string PhoneNumber
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SetState(value);
            }
        }

        public string PhoneNumber2
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SetState(value);
            }
        }

        public string EmailAddress
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SetState(value);
            }
        }

        public string EmailAddress2
        {
            get
            {
                return GetState(string.Empty);
            }
            set
            {
                SetState(value);
            }
        }

        public string ImagePath
        {
            get
            {
                return GetState("ms-appx:///Assets/Images/Guests/Person01.png");
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("ms-appx:///"))
                {
                    SetState(SqLiteManager.DefaultContactImagePath);
                }
                else
                {
                    SetState(value);
                }
            }
        }
        #endregion

        [NotMapped]
        public ImageSource Image
        {
            get
            {
                return new BitmapImage(new Uri(ImagePath));
            }
        }

        [NotMapped]
        public ContactCategoryModel Category
        {
            get
            {
                return GetState<ContactCategoryModel>();
            }

            set
            {
                SetState(value);
            }
        }

        public abstract RelayCommand EditCommand { get; }

        public abstract RelayCommand SaveChangesCommand { get; }

        public abstract RelayCommand CancelChangesCommand { get; }

        /// <summary>
        /// True if we want to edit contact information
        /// </summary>
        [NotMapped]
        public bool EditContact
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

        public override string SearchTerm { get; set; }

        /// <summary>
        /// Show search if tiled with all contacts page.
        /// </summary>
        public bool TiledWithAllContactsPage { get; set; }

        /// <summary>
        /// Get a category, or create one if it doesn' exist.
        /// </summary>
        protected ContactCategoryModel GetCategory(string categoryTitle)
        {
            foreach (var category in AppViewModel.AllContacts.Contacts.Categories)
            {
                if (category.Title == categoryTitle)
                    return category;
            }

            var newCategory = new ContactCategoryModel { Title = categoryTitle };
            AppViewModel.AllContacts.Contacts.Categories.Add(newCategory);

            return newCategory;
        }
    }
}
