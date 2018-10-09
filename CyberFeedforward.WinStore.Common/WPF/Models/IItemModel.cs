//------------------------------------------------------------
// <copyright file="IItemModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Common.WPF
{
    using Windows.UI.Xaml.Media;

    public interface IItemModel<TCategory> where TCategory: ICategoryModel
    {
        int ID { get; }

        TCategory Category { get; }

        string PageTitle { get; }
        
        ImageSource Image { get; }
    }
}
