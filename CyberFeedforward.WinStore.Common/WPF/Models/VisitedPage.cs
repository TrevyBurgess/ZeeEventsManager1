//------------------------------------------------------------
// <copyright file="VisitedPageLink.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using System.Collections.Generic;

    public class VisitedPage<TAppViewModel> where TAppViewModel : AppLevelViewModelBase<TAppViewModel>
    {
        /// <summary>
        /// Menu index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Menu associated with the saved page
        /// </summary>
        public List<MenuItem<TAppViewModel>> Menu { get; set; }

        /// <summary>
        /// Page title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Specify if back button should be shown
        /// </summary>
        public bool ShowBackLink { get; set; }
    }
}
