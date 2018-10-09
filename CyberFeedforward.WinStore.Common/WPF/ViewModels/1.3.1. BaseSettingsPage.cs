//------------------------------------------------------------
// <copyright file="BaseSettingsPage.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberFeedForward.WUP.Common.WPF
{
    public abstract class BaseSettingsPage<TAppViewModel> :
        PageViewmodelBase<TAppViewModel>
        where TAppViewModel : AppLevelViewModelBase<TAppViewModel>
    {
        public BaseSettingsPage(TAppViewModel appViewModel) : base(appViewModel)
        {
            IconCode = "Settings";
        }

        /// <summary>
        /// Gets or sets background image
        /// </summary>
        [NotMapped]
        public string BackgroundImage
        {
            get
            {
                return GetState(string.Empty, SaveType.RoamingSettings);
            }

            set
            {
                if (value.StartsWith("ms-appx:///"))
                {
                    SetState(value, SaveType.RoamingSettings);
                }
                else
                {
                    SetState("ms-appx:///" + value);
                }
            }
        }

        /// <summary>
        /// Gets or sets background opacity
        /// </summary>
        [NotMapped]
        public string BackgroundOpacity
        {
            get
            {
                return GetState("0.5", SaveType.RoamingSettings);
            }

            set
            {
                SetState(value, SaveType.RoamingSettings);
            }
        }
    }
}
