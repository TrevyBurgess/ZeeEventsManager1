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

    public sealed partial class CalendarDayNamesTemplate : UserControl
    {
        public CalendarDayModel CalendarDayModel { get { return DataContext as CalendarDayModel; } }

        public CalendarDayNamesTemplate()
        {
            InitializeComponent();

            DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
