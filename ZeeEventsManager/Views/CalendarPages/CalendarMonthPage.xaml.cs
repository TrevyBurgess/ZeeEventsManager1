//------------------------------------------------------------
// <copyright file="CalendarPage.xaml" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using System;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// DataTemplateSelector: http://codepb.com/datatemplateselector-in-xaml-versatile-xaml-controls/
    /// </summary>
    public sealed partial class CalendarMonthPage : Page
    {
        private CalendarModel calendarModel { get; set; }

        public CalendarMonthPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            calendarModel = e?.Parameter as CalendarModel;

            calendarModel.ImplimentsSearch = false;
        }

        private void CalendarGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var model = e.ClickedItem as CalendarDayModel;
            if (model.IsValidDay)
            {
                var date = new DateTime(
                        calendarModel.CurrentDate.Year,
                        calendarModel.CurrentDate.Month, model.Day,
                        DateTime.Now.Hour,
                        DateTime.Now.Minute < 30 ? 0 : 30,
                        0);

                var newEvent = NewEventItemModel.GetNewEvent(date, true);
                calendarModel.AppViewModel.GoTo(typeof(EventItemPage), newEvent);
            }
        }
    }
}
