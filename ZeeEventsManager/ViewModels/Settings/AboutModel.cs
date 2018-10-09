//------------------------------------------------------------
// <copyright file="AboutModel.cs" company="TrevyBurgess" >
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
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class AboutModel : BaseSettingsPage<AppLevelModel>
    {
        public AboutModel(AppLevelModel appViewModel) : base(appViewModel)
        {
            IconCode = "Message";
            PageTitle = Q.Resources.About_PageTitle;

            MainMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(Q.Resources.Navigation_Back, "Back" )
            };
            MainMenuIndex = -1;

            ResetSubMenuIndexOnNavigation = true;

            BottomMenu = new ObservableCollection<MenuItem<AppLevelModel>>()
            {
                new MenuItem<AppLevelModel>(Q.Resources.About_PageTitle, "Message" )
            };
            BottomMenuIndex = 0;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public override string SearchTerm { get; set; }
        
        public override Task<bool> OnNavigatingFrom()
        {
            return Task.Run(() => { return true; });
        }
    }
}
