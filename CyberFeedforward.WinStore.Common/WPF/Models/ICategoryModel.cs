//------------------------------------------------------------
// <copyright file="ICategoryModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using Windows.UI.Xaml.Media;

    public interface ICategoryModel
    {
        string Title { get; set; }

        string ImagePath { get; set; }

        ImageSource Image { get; }
    }
}
