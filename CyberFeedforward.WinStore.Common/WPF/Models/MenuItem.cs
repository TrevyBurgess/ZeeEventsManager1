//------------------------------------------------------------
// <copyright file="BurgerItem.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System;

    /// <summary>
    /// menu items define for navigating program.
    /// - If <code>MenuItem(string, string, RelayCommand)</code> used,
    /// menu item will act as a command
    /// If <code>MenuItem(viewModel, Type, RelayCommand, RelayCommand, string, string)</code> used,
    /// menu item will allow navigation,
    /// with optional before and after commands
    /// </summary>
    /// <typeparam name="TAppViewModel"></typeparam>
    public class MenuItem<TAppViewModel> where TAppViewModel : AppLevelViewModelBase<TAppViewModel>
    {
        /// <summary>
        /// If commands are null, link will act as a back button.
        /// ELSE command is executed
        /// </summary>
        public MenuItem(
            string menuTitle,
            string menuIcon,
            RelayCommand command = null,
            bool reentry = false)
        {
            MenuTitle = menuTitle;
            MenuIcon = menuIcon;
            Command = command;
            BeforeNavigatingCommand = null;
            AfterNavigatingCommand = Command;
            Reentry = reentry;

            if (command == null)
            {
                UseBackButton = true;
                IsCommand = false;
            }
            else
            {
                UseBackButton = false;
                IsCommand = true;
            }
        }

        /// <summary>
        /// Specify page you want to navigate to, and associated fields,
        /// such as menu, title, etc.
        /// </summary>
        /// <param name="viewModel">Page's viewmodel. You may set viewModel using BeforeNavigatingCommand</param>
        /// <param name="page">Navigation page</param>
        public MenuItem(
            PageViewmodelBase<TAppViewModel> viewModel,
            Type page,
            RelayCommand afterNavigatingCommand = null,
            RelayCommand beforerNavigatingCommand = null,
            string menuTitle = null,
            string menuIcon = null,
            bool reentry = false)
        {
            MenuTitle = menuTitle == null ? viewModel?.PageTitle : menuTitle;
            MenuIcon = menuIcon == null ? viewModel?.IconCode : menuIcon;
            Page = page;
            ViewModel = viewModel;
            UseBackButton = false;
            IsCommand = false;
            BeforeNavigatingCommand = beforerNavigatingCommand;
            AfterNavigatingCommand = afterNavigatingCommand;
            Command = afterNavigatingCommand;
            Reentry = reentry;
        }

        /// <summary>
        /// Page to navigate to
        /// </summary>
        public Type Page { get; }

        /// <summary>
        /// View model associated with the page
        /// </summary>
        public PageViewmodelBase<TAppViewModel> ViewModel { get; set; }

        /// <summary>
        /// If true, use back function, else navigate to specified page
        /// </summary>
        public bool UseBackButton { get; }

        public bool IsCommand { get; }

        /// <summary>
        /// If true, allow user to enter page with same viewmodel type
        /// </summary>
        public bool Reentry { get; }

        /// <summary>
        /// Executed before nagigating to page
        /// </summary>
        public RelayCommand BeforeNavigatingCommand { get; }

        /// <summary>
        /// Executed after nagigating to page
        /// </summary>
        public RelayCommand AfterNavigatingCommand { get; }

        /// <summary>
        /// If <code>IsCommand==true</code>, execute if not null.
        /// Else navigate back
        /// </summary>
        public RelayCommand Command { get; }

        public string MenuTitle { get; }

        public string MenuIcon { get; }
    }
}