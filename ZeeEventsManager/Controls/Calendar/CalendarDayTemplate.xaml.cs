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

    public sealed partial class CalendarDayTemplate : UserControl
    {
        public CalendarDayModel CalendarDayModel { get { return DataContext as CalendarDayModel; } }
        
        public CalendarDayTemplate()
        {
            InitializeComponent();

            DataContextChanged += (s, e) => Bindings.Update();
        }

        private void EventEntry_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as DayEvent;
            item.Event.AppViewModel.GoTo(typeof(EventItemPage), item.Event, item.Event.ViewEventCommand);
        }
    }
}
