//------------------------------------------------------------
// <copyright file="ContactCategoryModel.cs" company="TrevyBurgess" >
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
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Media.Imaging;

    /// <summary>
    /// Used to group contacts by either first or last name
    /// </summary>
    public class ContactCategoryModel : ICategoryModel
    {
        public ImageSource Image
        {
            get
            {
                return new BitmapImage(new Uri(ImagePath));
            }
        }

        public string ImagePath
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Title { get; set; }
    }
}
