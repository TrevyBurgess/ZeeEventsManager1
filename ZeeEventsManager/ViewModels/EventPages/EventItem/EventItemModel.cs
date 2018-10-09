//------------------------------------------------------------
// <copyright file="EventItemModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//-----------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System;

    public class EventItemModel : BaseEventItemModel
    {
        public EventItemModel(AppLevelModel appViewModel, DateTime newEventDate, bool NavFromCalendar) :
            base(appViewModel, newEventDate, NavFromCalendar)
        {
            ViewEventCommand.Execute();
        }

        #region Manage Event
        protected override bool EventCancelAction()
        {
            if (!EditAttendance)
            {
                EditEvent = false;

                EventTitle = meetingBackup.EventTitle;
                Venue = meetingBackup.Venue;
                Description = meetingBackup.Description;
                EventBegin = meetingBackup.EventStart;
                EventEnd = meetingBackup.EventEnd;
                ImagePath = meetingBackup.ImagePath;
                VenueContactEmail = meetingBackup.VenueContactEmail;
                VenueContactPhone = meetingBackup.VenueContactPhone;
            }
            else
            {
            }

            return true;
        }

        protected override void EventSaveAction()
        {
            // Restore sub-menu
            SubMenu.Clear();
            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditAttendance, "Contact2", EditAttendanceCommand));
            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditEvent, "Edit", EditEventCommand));
            EditEvent = false;

            AppViewModel.Calendar.AddOrUpdateEvent(this);
        }
        #endregion
    }
}
