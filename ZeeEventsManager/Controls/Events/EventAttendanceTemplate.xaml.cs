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

    public sealed partial class EventAttendanceTemplate : UserControl
    {
        public EventGuest eventGuest { get { return DataContext as EventGuest; } }

        public EventAttendanceTemplate()
        {
            InitializeComponent();

            DataContextChanged += (s, e) => Bindings.Update();
        }
    }
}
