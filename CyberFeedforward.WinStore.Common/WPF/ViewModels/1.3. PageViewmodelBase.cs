//------------------------------------------------------------
// <copyright file="PageViewmodelBase.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Threading.Tasks;

    /// <summary>
    /// Viewmodel base class for all page related viewmodels
    /// </summary>
    /// <typeparam name="TAppViewModel">Viewmodel for main app</typeparam>
    public abstract class PageViewmodelBase<TAppViewModel> :
        ViewModelBase
        where TAppViewModel : AppLevelViewModelBase<TAppViewModel>
    {
        public PageViewmodelBase(TAppViewModel appViewModel)
        {
            AppViewModel = appViewModel;

            SubMenu.CollectionChanged += (s, e) =>
            {
                if (SubMenu.Count == 0)
                    ShowSecondaryMenu = false;
                else
                    ShowSecondaryMenu = true;
            };
        }

        [NotMapped]
        public TAppViewModel AppViewModel { get; }

        #region Menus
        /// <summary>
        /// Main menu to display for this page
        /// </summary>
        [NotMapped]
        public ObservableCollection<MenuItem<TAppViewModel>> MainMenu
        {
            get
            {
                return GetState<ObservableCollection<MenuItem<TAppViewModel>>>();
            }

            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Selected index of the menu corresponding to the current page
        /// </summary>
        [NotMapped]
        public int MainMenuIndex
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

        /// <summary>
        /// Specify if sub-menu index should be reset to -1 whenever we navifate to the page. 
        /// </summary>
        [NotMapped]
        public bool ResetMainMenuIndexOnNavigation
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

        /// <summary>
        /// Sub menu to display for this page
        /// </summary>
        [NotMapped]
        public ObservableCollection<MenuItem<TAppViewModel>> SubMenu
        {
            get
            {
                return GetState(new ObservableCollection<MenuItem<TAppViewModel>>());
            }

            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Menu icon. Default is an exclaimation point.
        /// </summary>
        [NotMapped]
        public bool ShowSecondaryMenu
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

        /// <summary>
        /// Gets or sets the selected secondary menu index
        /// </summary>
        [NotMapped]
        public int SubMenuIndex
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

        /// <summary>
        /// Specify if sub-menu index should be reset to -1 whenever we navifate to the page. 
        /// </summary>
        [NotMapped]
        public bool ResetSubMenuIndexOnNavigation
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

        /// <summary>
        /// Allows users to navigate to various pages
        /// </summary>
        public ObservableCollection<MenuItem<TAppViewModel>> BottomMenu
        {
            get
            {
                return GetState<ObservableCollection<MenuItem<TAppViewModel>>>();
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Selected index for bottom menu
        /// </summary>
        [NotMapped]
        public int BottomMenuIndex
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

        /// <summary>
        /// Specify if sub-menu index should be reset to -1 whenever we navifate to the page. 
        /// </summary>
        [NotMapped]
        public bool ResetBottomMenuIndexOnNavigation
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
        #endregion

        #region Search
        /// <summary>
        /// Indicates if search is implimented.
        /// </summary>
        [NotMapped]
        public bool ImplimentsSearch
        {
            get
            {
                return GetState(false);
            }

            set
            {
                SetState(value);
                AppViewModel.ImplimentsSearch = value;
            }
        }

        /// <summary>
        /// For search if needed
        /// </summary>
        public abstract string SearchTerm { get; set; }
        #endregion

        /// <summary>
        /// Menu icon. Default is an exclaimation point.
        /// </summary>
        [NotMapped]
        public string IconCode
        {
            get
            {
                return GetState("Important");
            }

            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// NOT USED YET
        /// </summary>
        [NotMapped]
        public string IconOverlayCode
        {
            get
            {
                return GetState<string>();
            }

            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Page title
        /// </summary>
        [NotMapped]
        public string PageTitle
        {
            get
            {
                return GetState("TITLE NOT SET");
            }

            set
            {
               SetState(value);
            }
        }

        /// <summary>
        /// Set to true if this is the first page 
        /// </summary>
        [NotMapped]
        public bool IsInitialAppPage
        {
            get
            {
                return GetState(false);
            }

            protected set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Indicate if page data has changed and needs to be saved
        /// </summary>
        [NotMapped]
        public bool DataChanged
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

        /// <summary>
        /// Called navigating From a specified page
        /// </summary>
        /// <returns>If false, request back navigation to be cancelled</returns>
        public abstract Task<bool> OnNavigatingFrom();

        /// <summary>
        /// Called before navigating to a specified page
        /// </summary>
        public virtual void OnNavigatingTo() { }
    }
}
