//------------------------------------------------------------
// <copyright file="NewEventItemModel.cs" company="TrevyBurgess" >
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

    public class NewEventItemModel : BaseEventItemModel
    {
        private static NewEventItemModel newEvent;

        public static NewEventItemModel GetNewEvent(DateTime eventStartDate, bool navFromCalendar)
        {
            if (AppLevelModel.ViewModel.NewEventId == -1 || newEvent == null)
            {
                newEvent = new NewEventItemModel(
                    AppLevelModel.ViewModel, eventStartDate, navFromCalendar);
            }
            else
            {
                newEvent.ResetEvent();
            }

            if (newEvent.ID == -1)
                newEvent.ID = AppLevelModel.ViewModel.NewEventId;

            newEvent.EditEvent = true;

            return newEvent;
        }

        /// <summary>
        /// Creates a new event and saves a copy
        /// </summary>
        private NewEventItemModel(AppLevelModel appViewModel, DateTime eventStartDate, bool navFromCalendar) :
            base(appViewModel, eventStartDate, navFromCalendar)
        {
            EditEvent = true;
            IsNewEvent = true;

            // Override base icons
            IconCode = "Add";
            PageTitle = Q.Resources.NewEvent_PageTitle;

            SubMenu.Clear();
            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_EditAttendance, "Contact2", EditAttendanceCommand));
            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_SaveEvent, "Save", SaveEventCommand));
            SubMenu.Add(new MenuItem<AppLevelModel>(Q.Resources.Event_DiscardChanges, "Cancel", CancelEventChangesCommand));

            SqLiteManager.RetriveNewEvent(this);

            // Create backup
            meetingBackup = new Meeting
            {
                EventTitle = EventTitle,
                Venue = Venue,
                Description = Description,
                EventStart = EventBegin,
                EventEnd = EventBegin,
                ImagePath = ImagePath,
                VenueContactEmail = VenueContactEmail,
                VenueContactPhone = VenueContactPhone,
                MeetingID = ID
            };
        }

        #region Manage Event
        protected override bool EventCancelAction()
        {
            ResetEvent();
            AppViewModel.GoBack();

            return true;
        }

        private void ResetEvent()
        {
            EditEvent = false;
            EditAttendance = false;
            AttendanceListModified = false;
            EventTitle = meetingBackup.EventTitle;
            Venue = meetingBackup.Venue;
            Description = meetingBackup.Description;

            EventBegin = DateTime.Now;
            EventEnd = DateTime.Now.AddHours(1);

            ImagePath = meetingBackup.ImagePath;
            VenueContactEmail = meetingBackup.VenueContactEmail;
            VenueContactPhone = meetingBackup.VenueContactPhone;
            ID = meetingBackup.MeetingID;
        }

        /// <summary>
        /// Call calendar to add event
        /// </summary>
        protected override void EventSaveAction()
        {
            AppViewModel.Calendar.AddOrUpdateEvent(newEvent);
            EditEvent = false;
            IsNewEvent = false;
            AppViewModel.GoBack();
        }
        #endregion
    }
}