//------------------------------------------------------------
// <copyright file="CalendarDayTemplate.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//-----------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Windows.UI.Xaml.Controls;

    public sealed partial class ContactTemplate : UserControl
    {
        public ContactItemModel contact { get { return DataContext as ContactItemModel; } }

        public ContactTemplate()
        {
            InitializeComponent();

            DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
