//------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Windows.UI.Core;
    using Windows.UI.Xaml;

    /// <summary>
    /// Base app-level viewmodel for Windows Store App
    /// </summary>
    public abstract class AppLevelViewModelBase<TAppViewModel> :
        ViewModelBase
        where TAppViewModel : AppLevelViewModelBase<TAppViewModel>
    {
        #region Search
        public string SearchBoxText
        {
            get
            {
                return GetState<string>();
            }
            set
            {
                if (SetState(value))
                {
                    if (ChildViewmodel != null)
                    {
                        ChildViewmodel.SearchTerm = value;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates if search is implimented.
        /// </summary>
        public bool ImplimentsSearch
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
        /// Search box child page may use to search for items
        /// </summary>
        public List<string> SearchBoxItems
        {
            get
            {
                return GetState<List<string>>();
            }
            set
            {
                SetState(value);
            }
        }
        #endregion

        #region About program
        /// <summary>
        /// Email address to use when sending feedback
        /// </summary>
        public string FeedbackEmail
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
        /// App name to show in feedback message
        /// </summary>
        public string FeedbackAppName
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
        /// Subscribe to event to hear when messages need to be sent to user
        /// </summary>
        public event MessageEventHandler MessageToDisplay;

        /// <summary>
        /// Send message to user. For message results, see: <code>MessageDialogResults</code>
        /// </summary>
        /// <param name="message">Message to send</param>
        public void MessageDialog(string message, string title, MessageDialogOptions option)
        {
            MessageDialogResults = MessageDialogResultsEnum.No_Results;
            MessageToDisplay?.Invoke(message, title, option);
        }

        /// <summary>
        /// User selection when user responds to ShowMessage dialog
        /// </summary>
        public MessageDialogResultsEnum MessageDialogResults { get; set; }
        #endregion

        #region Navigation
        /// <summary>
        /// Indicates if we can navigate back
        /// </summary>
        public bool CanGoBack
        {
            get
            {
                return GetState(true);
            }
            set
            {
                if (SetState(value))
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = value ?
                        AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// The Frame element maintains a log of visited pages, but not its viewmodels.
        /// This maintains the viewmodel list
        /// </summary>
        public Stack<PageViewmodelBase<TAppViewModel>> VisitedPages
        {
            get
            {
                return GetState(new Stack<PageViewmodelBase<TAppViewModel>>());
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Send message to navigate back to previous page
        /// </summary>
        public event NavigateBackEventHandler NavigateBack;

        /// <summary>
        /// Send message to navigate to specified page
        /// </summary>
        public event NavigateEventHandler<TAppViewModel> NavigateTo;

        /// <summary>
        /// Send command to navigate back
        /// </summary>
        public void GoBack()
        {
            NavigateBack?.Invoke();
        }

        /// <summary>
        /// Navigate to specified page
        /// </summary>
        public void GoTo(
            Type page,
            PageViewmodelBase<TAppViewModel> viewModel,
            RelayCommand command = null,
            bool allowReentry = false)
        {
            NavigateTo?.Invoke(page, viewModel, command, allowReentry);
        }
        #endregion

        #region Menus
        /// <summary>
        /// Specify the top-level menu for the app
        /// </summary>
        public ObservableCollection<MenuItem<TAppViewModel>> TopLevelMenu
        {
            get
            {
                return GetState(new ObservableCollection<MenuItem<TAppViewModel>>());
            }
        }
        #endregion

        /// <summary>
        /// Viewmodel associated with page being hosted
        /// </summary>
        public PageViewmodelBase<TAppViewModel> ChildViewmodel
        {
            get
            {
                return GetState<PageViewmodelBase<TAppViewModel>>();
            }
            set
            {
                try
                {
                    SetState(value);
                }
                catch
                {
                    SetState(value);
                }
            }
        }

        #region App Dimensions
        public double ApplicationWidth
        {
            get
            {
                return GetState<double>();
            }
            set
            {
                SetState(value);
            }
        }

        public double ApplicationHeight
        {
            get
            {
                return GetState<double>();
            }
            set
            {
                SetState(value);
            }
        }









        #endregion
    }

    public delegate void MessageEventHandler(string content, string title, MessageDialogOptions option);

    public delegate void NavigateBackEventHandler();

    public delegate void NavigateEventHandler<TAppViewModel>
        (Type page,
        PageViewmodelBase<TAppViewModel> viewModel,
        RelayCommand command,
        bool allowReentry)
        where TAppViewModel : AppLevelViewModelBase<TAppViewModel>;
}

