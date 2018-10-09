//------------------------------------------------------------
// <copyright file="HamburgerMainPage.xaml" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System;
    using Windows.UI.Core;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Navigation;
    using System.Threading.Tasks;
    using Social.ZeeEventsManager;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// Icons: https://msdn.microsoft.com/en-us/library/windows/apps/jj841126.aspx
    /// </summary>
    public sealed partial class HamburgerMainPage : Page
    {
        private AppLevelModel AppViewModel;

        public HamburgerMainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            AppViewModel = e?.Parameter as AppLevelModel;

            AppViewModel.MessageToDisplay += AppViewModel_MessageToDisplay;

            AppViewModel.NavigateBack -= NavigateBack;
            AppViewModel.NavigateBack += NavigateBack;

            AppViewModel.NavigateTo += (page, viewModel, command, reentry) =>
            {
                if (reentry || viewModel != AppViewModel?.ChildViewmodel)
                {
                    viewModel.OnNavigatingTo();
                    MainPaneFrame.Navigate(page, viewModel);
                    AppViewModel.VisitedPages.Push(viewModel);
                    AppViewModel.ChildViewmodel = viewModel;
                    AppViewModel.CanGoBack = !viewModel.IsInitialAppPage;
                    ContentSplitView.IsPaneOpen = false;
                    command?.Execute();
                }
            };
        }

        private async void AppViewModel_MessageToDisplay(
            string message, string title, MessageDialogOptions option)
        {
            MessageDialog messageDialog = new MessageDialog(message, title);
            switch (option)
            {
                case MessageDialogOptions.Okay_Cancel:
                    messageDialog.Commands.Add(new UICommand(Q.Resources.MessageDialog_Okay)
                    {
                        Id = MessageDialogResultsEnum.Okay
                    });
                    messageDialog.Commands.Add(new UICommand(Q.Resources.MessageDialog_Cancel)
                    {
                        Id = MessageDialogResultsEnum.Cancel
                    });
                    messageDialog.DefaultCommandIndex = 0;
                    messageDialog.CancelCommandIndex = 1;
                    break;

                case MessageDialogOptions.OK:
                    messageDialog.Commands.Add(new UICommand(Q.Resources.MessageDialog_Okay)
                    {
                        Id = MessageDialogResultsEnum.Okay
                    });
                    messageDialog.DefaultCommandIndex = 0;
                    break;

                default:
                    throw new NotSupportedException();
            }

            var res = await messageDialog.ShowAsync();

            AppViewModel.MessageDialogResults =
                (MessageDialogResultsEnum)(res).Id;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        /// <summary>
        /// Try to navigate back
        /// </summary>
        private async void NavigateBack()
        {
            if (!navInProgress)
            {
                navInProgress = true;
                await TryNavagatingBack();
                navInProgress = false;
            }
        }

        bool navInProgress;

        /// <summary>
        /// Try to navigate Back
        /// </summary>
        /// <returns>True if operation successful</returns>
        private async Task<bool> TryNavagatingBack()
        {
            if (MainPaneFrame.CanGoBack && AppViewModel.VisitedPages.Count > 1)
            {
                // Get previous viewmodel
                var prevPage = AppViewModel.VisitedPages.Pop();

                // Perform any cleanup before navigating away.
                if (await prevPage.OnNavigatingFrom())
                {
                    // We can navigate back. Perform cleanup and navigate back
                    if (AppViewModel.ChildViewmodel != null)
                    {
                        AppViewModel.ChildViewmodel = AppViewModel.VisitedPages.Peek();

                        // Make sure any submenus are not unnecessarily selected
                        if (AppViewModel.ChildViewmodel.ResetMainMenuIndexOnNavigation)
                            AppViewModel.ChildViewmodel.MainMenuIndex = -1;

                        if (AppViewModel.ChildViewmodel.ResetSubMenuIndexOnNavigation)
                            AppViewModel.ChildViewmodel.SubMenuIndex = -1;

                        if (AppViewModel.ChildViewmodel.ResetBottomMenuIndexOnNavigation)
                            AppViewModel.ChildViewmodel.BottomMenuIndex = -1;
                    }

                    AppViewModel.CanGoBack = !AppViewModel.ChildViewmodel.IsInitialAppPage;

                    // Go to previous page
                    MainPaneFrame.GoBack();

                    return true;
                }
                else
                {
                    // Navigation cancelled. Restore navigation stack.
                    AppViewModel.VisitedPages.Push(prevPage);
                    return false;
                }
            }
            else
            {
                AppViewModel.CanGoBack = false;
                return false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContentSplitView.IsPaneOpen = false;
            NavigationHelper(AppViewModel.TopLevelMenu[0], -1);

            SystemNavigationManager.GetForCurrentView().BackRequested += HamburgerMainPage_BackRequested;
        }

        private async void HamburgerMainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = await TryNavagatingBack();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            ContentSplitView.IsPaneOpen = !ContentSplitView.IsPaneOpen;
        }

        private async void SendFrown_Click(object sender, RoutedEventArgs e)
        {
            var smiley = new SmileyDialog(
                AppViewModel.FeedbackEmail,
                AppViewModel.FeedbackAppName,
                SettingsModel.HappyFace,
                SettingsModel.SadFace)
            {
                ShowSmileMessage = false
            };

            await smiley.ShowAsync();
        }

        private async void SendSmile_Click(object sender, RoutedEventArgs e)
        {
            var smiley = new SmileyDialog(
                AppViewModel.FeedbackEmail,
                AppViewModel.FeedbackAppName,
                SettingsModel.HappyFace,
                SettingsModel.SadFace)
            {
                ShowSmileMessage = true
            };

            await smiley.ShowAsync();
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AppViewModel.ApplicationWidth = MainPage.ActualWidth;
            AppViewModel.ApplicationHeight = MainPage.ActualHeight;
        }

        private void MenuListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentSplitView.IsPaneOpen = false;
            var listBox = (sender as ListBox);
            var item = listBox.SelectedItem as MenuItem<AppLevelModel>;
            if (item.IsCommand)
            {
                if (item.Command != null && item.Command.CanExecute())
                    item.Command.Execute();
            }
            else
            {
                NavigationHelper(item, listBox.SelectedIndex);
            }

            if (AppViewModel.ChildViewmodel != null)
            {
                if (AppViewModel.ChildViewmodel.ResetMainMenuIndexOnNavigation)
                    AppViewModel.ChildViewmodel.MainMenuIndex = -1;

                if (AppViewModel.ChildViewmodel.ResetSubMenuIndexOnNavigation)
                    AppViewModel.ChildViewmodel.SubMenuIndex = -1;

                if (AppViewModel.ChildViewmodel.ResetBottomMenuIndexOnNavigation)
                    AppViewModel.ChildViewmodel.BottomMenuIndex = -1;
            }

            e.Handled = true;
        }

        private async void NavigationHelper(MenuItem<AppLevelModel> item, int currentIndex)
        {
            if (item.UseBackButton)
            {
                await TryNavagatingBack();
            }
            else
            {
                if (item.Reentry || item.ViewModel != AppViewModel?.ChildViewmodel)
                {
                    bool canNavigate;
                    if (AppViewModel.VisitedPages.Count == 0)
                    {
                        // No page to navigate away from
                        canNavigate = true;
                    }
                    else
                    {
                        // Allow page to perform any cleanup if necessary.
                        var currentPage = AppViewModel.VisitedPages.Peek();
                        canNavigate = await currentPage.OnNavigatingFrom();
                    }

                    if (canNavigate)
                    {
                        item.BeforeNavigatingCommand?.Execute();

                        MainPaneFrame.Navigate(item.Page, item.ViewModel);

                        AppViewModel.VisitedPages.Push(item.ViewModel);
                        AppViewModel.ChildViewmodel = item.ViewModel;
                        AppViewModel.CanGoBack = !item.ViewModel.IsInitialAppPage;

                        item.AfterNavigatingCommand?.Execute();
                    }
                }
            }
        }
    }
}