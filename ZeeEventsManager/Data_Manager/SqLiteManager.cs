//------------------------------------------------------------
// <copyright file="SqLiteManager.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Windows.Data.Json;
    using Windows.Storage;

    /// <summary>
    /// SQLite for UWP: http://www.sqlite.org/2016/sqlite-uwp-3120000.vsix
    /// http://www.shenchauhan.com/blog/2015/11/25/sqlite-and-uwp
    /// </summary>
    public static class SqLiteManager
    {
        public const string DefaultEventImagePath = "ms-appx:///Assets/Images/Events/Event01.png";
        public const string DefaultContactImagePath = "ms-appx:///Assets/Images/Guests/Person01.png";
        public const string SQLiteFileName = "EventsSqLite.db";
        public static string SqLiteDatabasePath;

        static SqLiteManager()
        {
            SqLiteDatabasePath = Path.Combine(ApplicationData.Current.RoamingFolder.Path, SQLiteFileName);

            using (var db = new EventManagerDBContext())
            {
                if (db.Database.EnsureCreated())
                {
                    AppLevelModel.ViewModel.NewEventId = -1;
                }
            }
        }

        #region Contact stuff
        public static void GetAllContacts(AppLevelModel appViewModel, ContactCollectionModel allContacts)
        {
            using (var db = new EventManagerDBContext())
            {
                var dict = new Dictionary<string, ContactCategoryModel>();
                foreach (var contact in db.Contacts)
                {
                    // Get contact category name
                    var catName = appViewModel.Settings.UserNameInStandardFormat.Value
                        ? contact.LastName[0].ToString().ToUpper() : contact.FirstName[0].ToString().ToUpper();

                    // Get category, or create one if it doesn't exist. 
                    ContactCategoryModel category;
                    if (dict.ContainsKey(catName))
                    {
                        category = dict[catName];
                    }
                    else
                    {
                        category = new ContactCategoryModel { Title = catName.ToUpper() };
                        dict.Add(catName, category);
                        allContacts.Categories.Add(category);
                    }

                    // Add to list of contacts
                    allContacts.Add(new ContactItemModel(appViewModel)
                    {
                        ID = contact.ContactID,
                        EmailAddress = contact.EmailAddress,
                        EmailAddress2 = contact.EmailAddress2,
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        PhoneNumber = contact.PhoneNumber,
                        PhoneNumber2 = contact.PhoneNumber2,
                        Category = category,
                        ImagePath = contact.ImagePath
                    });
                }
            }
        }

        /// <summary>
        /// Add or update contact
        /// </summary>
        public static int AddOrUpdateContact(BaseContactItemModel contactToAdd)
        {
            if (contactToAdd == null) throw new ArgumentNullException();

            using (var db = new EventManagerDBContext())
            {
                if (contactToAdd.ID < 0)
                {
                    var newContactModel = new Contact
                    {
                        EmailAddress = contactToAdd.EmailAddress,
                        EmailAddress2 = contactToAdd.EmailAddress2,
                        FirstName = contactToAdd.FirstName,
                        LastName = contactToAdd.LastName,
                        PhoneNumber = contactToAdd.PhoneNumber,
                        PhoneNumber2 = contactToAdd.PhoneNumber2,
                        ImagePath = contactToAdd.ImagePath
                    };

                    db.Contacts.Add(newContactModel);
                    db.SaveChanges();
                    //     SaveDatabase();
                    contactToAdd.ID = newContactModel.ContactID;

                    contactToAdd.AppViewModel.ResetNewContact();

                    return newContactModel.ContactID;
                }
                else
                {
                    var query = from contact in db.Contacts
                                where contact.ContactID == contactToAdd.ID
                                select contact;
                    var returnedContact = query.First();

                    if (returnedContact == null)
                        throw new IndexOutOfRangeException($"Event index {contactToAdd.ID} not found");

                    returnedContact.EmailAddress = contactToAdd.EmailAddress;
                    returnedContact.EmailAddress2 = contactToAdd.EmailAddress2;
                    returnedContact.FirstName = contactToAdd.FirstName;
                    returnedContact.LastName = contactToAdd.LastName;
                    returnedContact.PhoneNumber = contactToAdd.PhoneNumber;
                    returnedContact.PhoneNumber2 = contactToAdd.PhoneNumber2;
                    returnedContact.ImagePath = contactToAdd.ImagePath;

                    db.Contacts.Update(returnedContact);
                    db.SaveChanges();

                    contactToAdd.AppViewModel.ResetNewContact();
                    //     SaveDatabase();
                    return returnedContact.ContactID;
                }
            }
        }
        #endregion

        #region New Event management
        /// <summary>
        /// Retrieve new event from database or create one if doesn't exist.
        /// </summary>
        public static void RetriveNewEvent(NewEventItemModel newEventContainer, bool retrieveTempEvent = false)
        {
            using (var db = new EventManagerDBContext())
            {
                if (AppLevelModel.ViewModel.NewEventId < 0)
                {
                    var newEvent = new Meeting
                    {
                        EventTitle = newEventContainer.EventTitle,
                        Venue = newEventContainer.Venue,
                        Description = newEventContainer.Description,
                        EventStart = newEventContainer.EventBegin,
                        EventEnd = newEventContainer.EventEnd,
                        ImagePath = newEventContainer.ImagePath,
                        VenueContactEmail = newEventContainer.VenueContactEmail,
                        VenueContactPhone = newEventContainer.VenueContactPhone
                    };

                    db.Events.Add(newEvent);
                    db.SaveChanges();

                    newEventContainer.ID = newEvent.MeetingID;
                    AppLevelModel.ViewModel.NewEventId = newEvent.MeetingID;
                }
                else
                {
                    var query = from event1 in db.Events
                                where event1.MeetingID == AppLevelModel.ViewModel.NewEventId
                                select event1;

                    var result = query.FirstOrDefault();
                    if (result == null)
                    {
                        // No events found. Shouldn't occur.
                        AppLevelModel.ViewModel.NewEventId = -1;
                        RetriveNewEvent(newEventContainer);

                        result = query.FirstOrDefault();
                    }

                    newEventContainer.ID = result.MeetingID;

                    if (retrieveTempEvent)
                    {
                        newEventContainer.EventTitle = result.EventTitle;
                        newEventContainer.Venue = result.Venue;
                        newEventContainer.Description = result.Description;
                        newEventContainer.EventBegin = result.EventStart;
                        newEventContainer.EventEnd = result.EventEnd;
                        newEventContainer.ImagePath = result.ImagePath;
                        newEventContainer.VenueContactEmail = result.VenueContactEmail;
                        newEventContainer.VenueContactPhone = result.VenueContactPhone;
                    }
                    else
                    {
                        // Delete guest list
                        var guestQuery = from eventGuest in db.EventGuests
                                         where eventGuest.MeetingID == result.MeetingID
                                         select eventGuest;

                        foreach (var guest in guestQuery)
                        {
                            db.EventGuests.Remove(guest);
                        }

                        db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Add or update contact
        /// </summary>
        public static int SaveNewEvent(NewEventItemModel theEvent)
        {
            using (var db = new EventManagerDBContext())
            {
                // We are creating a new event entry with default values
                if (AppLevelModel.ViewModel.NewEventId < 0)
                {
                    var newEventModel = new Meeting
                    {
                        EventTitle = theEvent.EventTitle,
                        Venue = theEvent.Venue,
                        Description = theEvent.Description,
                        EventStart = theEvent.EventBegin,
                        EventEnd = theEvent.EventEnd,
                        ImagePath = theEvent.ImagePath,
                        VenueContactEmail = theEvent.VenueContactEmail,
                        VenueContactPhone = theEvent.VenueContactPhone
                    };

                    db.Events.Add(newEventModel);
                    db.SaveChanges();

                    theEvent.ID = newEventModel.MeetingID;
                    AppLevelModel.ViewModel.NewEventId = newEventModel.MeetingID;

                    return newEventModel.MeetingID;
                }
                else
                {
                    // We are saving new event. Event no longer new.
                    var query = from event1 in db.Events
                                where event1.MeetingID == AppLevelModel.ViewModel.NewEventId
                                select event1;
                    var returnedEvent = query.FirstOrDefault();

                    returnedEvent.EventTitle = theEvent.EventTitle;
                    returnedEvent.Venue = theEvent.Venue;
                    returnedEvent.Description = theEvent.Description;
                    returnedEvent.EventStart = theEvent.EventBegin;
                    returnedEvent.EventEnd = theEvent.EventEnd;
                    returnedEvent.ImagePath = theEvent.ImagePath;
                    returnedEvent.VenueContactEmail = theEvent.VenueContactEmail;
                    returnedEvent.VenueContactPhone = theEvent.VenueContactPhone;

                    db.Events.Update(returnedEvent);
                    db.SaveChanges();

                    theEvent.ID = returnedEvent.MeetingID;
                    AppLevelModel.ViewModel.NewEventId = -1;

                    return returnedEvent.MeetingID;
                }
            }
        }
        #endregion

        #region Populate calendar
        internal static CalendarDayModel GetCalendarDay(DateTime currentDate, AppLevelModel appViewModel)
        {
            var day = new CalendarDayModel(currentDate.Day, true, currentDate.DayOfWeek.ToString());

            using (var db = new EventManagerDBContext())
            {
                var startDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                var endDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).AddDays(1);

                var query = from theEvent in db.Events
                            where !(theEvent.EventEnd <= startDate || theEvent.EventStart >= endDate)
                            select theEvent;

                foreach (Meeting item in query)
                {
                    var newEvent = new EventItemModel(appViewModel, item.EventStart, true)
                    {
                        EventTitle = item.EventTitle,
                        Venue = item.Venue,
                        Description = item.Description,
                        EventBegin = item.EventStart,
                        EventEnd = item.EventEnd,
                        VenueContactEmail = item.VenueContactEmail,
                        VenueContactPhone = item.VenueContactPhone,
                        ImagePath = item.ImagePath,
                        ID = item.MeetingID
                    };

                    day.DayEvents.Add(new DayEvent(newEvent, EventDurationType.SingleDay));

                    // Guests
                    var guestQuery = from eventGuest in db.EventGuests
                                     where eventGuest.MeetingID == newEvent.ID
                                     select eventGuest;

                    foreach (var guest in guestQuery)
                    {
                        newEvent.GuestListModel.GuestList.Items.Add(new EventGuest
                        {
                            Guest = appViewModel.AllContacts.Contacts.GetItem(guest.ContactID),
                            ID = guest.MeetingGuestID,
                            ContactID = guest.ContactID,
                            GuestStatus = guest.GuestStatus,
                        });
                    }
                }

                return day;
            }
        }

        /// <summary>
        /// Get events within same month. Ordered by day only.
        /// </summary>
        public static ObservableCollection<CalendarDayModel> GetCalendarMonth(
            int year, int month, AppLevelModel appViewModel)
        {
            var CalendarGroup = new ObservableCollection<CalendarDayModel>();

            // Get first day of week
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var firstDayIndex = (int)firstDayOfMonth.DayOfWeek + 1;
            firstDayIndex = firstDayIndex == 7 ? 0 : firstDayIndex;
            firstDayIndex--;

            for (int i = 0; i < firstDayIndex; i++)
            {
                CalendarGroup.Add(new CalendarDayModel(0, false, string.Empty));
            }

            var lastDayIndex = firstDayOfMonth.AddMonths(1).AddDays(-1).Day + firstDayIndex;
            for (int i = firstDayIndex; i < lastDayIndex; i++)
            {
                var day = new DateTime(year, month, i - firstDayIndex + 1);
                var dayName = day.DayOfWeek.ToString();

                CalendarGroup.Add(new CalendarDayModel(i - firstDayIndex + 1, true, dayName));
            }

            for (int i = lastDayIndex; i < 42; i++)
            {
                CalendarGroup.Add(new CalendarDayModel(0, false, string.Empty));
            }

            firstDayIndex--;
            using (var db = new EventManagerDBContext())
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1);
                var query = from theEvent in db.Events
                            where !(theEvent.EventEnd <= startDate || theEvent.EventStart >= endDate)
                            select theEvent;

                foreach (Meeting item in query)
                {
                    if (item.MeetingID != appViewModel.NewEventId)
                    {
                        var newEvent = new EventItemModel(appViewModel, item.EventStart, true)
                        {
                            EventTitle = item.EventTitle,
                            Venue = item.Venue,
                            Description = item.Description,
                            EventBegin = item.EventStart,
                            EventEnd = item.EventEnd,
                            VenueContactEmail = item.VenueContactEmail,
                            VenueContactPhone = item.VenueContactPhone,
                            ImagePath = item.ImagePath,
                            ID = item.MeetingID
                        };

                        AddEventToCalendar(CalendarGroup, newEvent, firstDayOfMonth, lastDayOfMonth, firstDayIndex);

                        // Guests
                        var guestQuery = from eventGuest in db.EventGuests
                                         where eventGuest.MeetingID == newEvent.ID
                                         select eventGuest;

                        foreach (var guest in guestQuery)
                        {
                            newEvent.GuestListModel.GuestList.Items.Add(new EventGuest
                            {
                                Guest = appViewModel.AllContacts.Contacts.GetItem(guest.ContactID),
                                ID = guest.MeetingGuestID,
                                ContactID = guest.ContactID,
                                GuestStatus = guest.GuestStatus,
                            });
                        }
                    }
                }

                return CalendarGroup;
            }
        }

        private static void AddEventToCalendar(
            ObservableCollection<CalendarDayModel> calendarGroup,
            EventItemModel newEvent,
            DateTime firstDayOfMonth,
            DateTime lastDayOfMonth,
            int offset)
        {
            int start;
            if (newEvent.EventBegin < firstDayOfMonth)
                start = 1;
            else
                start = newEvent.EventBegin.Day;

            int end;
            if (newEvent.EventEnd > lastDayOfMonth)
                end = lastDayOfMonth.Day;
            else
                end = newEvent.EventEnd.Day;

            if (start == end)
            {
                calendarGroup[start + offset].
                    DayEvents.
                    Add(new DayEvent(newEvent, EventDurationType.None));
            }
            else
            {
                for (var day = start; day <= end; day++)
                {
                    DayEvent dayEvent;
                    if (day == start)
                        dayEvent = new DayEvent(newEvent, EventDurationType.BeginMultiday);
                    else if (day == end)
                        dayEvent = new DayEvent(newEvent, EventDurationType.EndMultiday);
                    else
                        dayEvent = new DayEvent(newEvent, EventDurationType.MiddleMultiday);

                    calendarGroup[day + offset].DayEvents.Add(dayEvent);
                }
            }
        }

        public static BaseEventItemModel GetLatestEvent(AppLevelModel appViewModel)
        {
            using (var db = new EventManagerDBContext())
            {
                var query = from event1 in db.Events
                            orderby event1.EventStart descending
                            where event1.EventStart > DateTime.Now
                            select event1;

                var result = query.FirstOrDefault();
                if (result == null)
                {
                    // No events found
                    return null;
                }
                else
                {
                    // Upcoming future event
                    var latestEvent = new EventItemModel(appViewModel, result.EventStart, false)
                    {
                        EventTitle = result.EventTitle,
                        Venue = result.Venue,
                        Description = result.Description,
                        EventBegin = result.EventStart,
                        EventEnd = result.EventEnd,
                        VenueContactEmail = result.VenueContactEmail,
                        VenueContactPhone = result.VenueContactPhone,
                        ImagePath = result.ImagePath,
                        ID = result.MeetingID
                    };

                    // Guests
                    var guestQuery = from eventGuest in db.EventGuests
                                     where eventGuest.MeetingID == result.MeetingID
                                     select eventGuest;

                    foreach (var guest in guestQuery)
                    {
                        latestEvent.GuestListModel.GuestList.Items.Add(new EventGuest
                        {
                            Guest = appViewModel.AllContacts.Contacts.GetItem(guest.ContactID),
                            ID = guest.MeetingGuestID,
                            GuestStatus = guest.GuestStatus,
                        });
                    }

                    return latestEvent;
                }
            }
        }
        #endregion

        #region Manage event attendance
        public static void RemoveGuestFromEvent(int eventGuestId)
        {
            using (var db = new EventManagerDBContext())
            {
                var query = from eventGuest in db.EventGuests
                            where eventGuest.MeetingGuestID == eventGuestId
                            select eventGuest;
                var result = query.FirstOrDefault();
                if (result != null)
                {
                    db.EventGuests.Remove(result);
                    db.SaveChanges();
                }
            }
        }

        public static EventGuest AddGuestToEvent(int eventId, ContactItemModel newGuest)
        {
            if (eventId < 0)
            {
                throw new ArgumentException($"{nameof(eventId)}: {eventId}");
            }
            else
            {
                using (var db = new EventManagerDBContext())
                {
                    // Look for event guest if it exists
                    var eventQuery = from eventGuest in db.EventGuests
                                     where eventGuest.ContactID == newGuest.ID &&
                                           eventGuest.MeetingID == eventId
                                     select eventGuest;
                    var result = eventQuery.FirstOrDefault();
                    if (result == null)
                    {
                        var meetingGuest = new MeetingGuest
                        {
                            ContactID = newGuest.ID,
                            MeetingID = eventId,
                            GuestStatus = GustArrivalStatus.Unknown
                        };

                        db.EventGuests.Add(meetingGuest);
                        db.SaveChanges();
                        var guest = new EventGuest
                        {
                            ContactID = newGuest.ID,
                            Guest = newGuest,
                            GuestStatus = GustArrivalStatus.Unknown,
                            ID = meetingGuest.MeetingGuestID
                        };

                        return guest;
                    }
                    else
                    {
                        return new EventGuest
                        {
                            ContactID = result.ContactID,
                            Guest = newGuest,
                            ID = result.ContactID,
                            GuestStatus = result.GuestStatus
                        };
                    }
                }
            }
        }

        internal static void DeleteEventAttendance(int eventId)
        {
            using (var db = new EventManagerDBContext())
            {
                var query = from eventGuest in db.EventGuests
                            where eventGuest.MeetingID == eventId
                            select eventGuest;

                foreach (var guest in query)
                {
                    db.EventGuests.Remove(guest);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Add or update contact
        /// </summary>
        public static int UpdateExistingEvent(EventItemModel eventToUpdate)
        {
            if (eventToUpdate == null) throw new ArgumentNullException();
            if (eventToUpdate.ID < 0) throw new ArgumentException($"Use {nameof(RetriveNewEvent)} to get new event");

            using (var db = new EventManagerDBContext())
            {
                // Get new ID
                var query = from event1 in db.Events
                            where event1.MeetingID == eventToUpdate.ID
                            select event1;
                var returnedEvent = query.FirstOrDefault();

                if (returnedEvent == null)
                    throw new IndexOutOfRangeException($"Event index {eventToUpdate.ID} not found");

                returnedEvent.EventTitle = eventToUpdate.EventTitle;
                returnedEvent.Venue = eventToUpdate.Venue;
                returnedEvent.Description = eventToUpdate.Description;
                returnedEvent.EventStart = eventToUpdate.EventBegin;
                returnedEvent.EventEnd = eventToUpdate.EventEnd;
                returnedEvent.ImagePath = eventToUpdate.ImagePath;
                returnedEvent.VenueContactEmail = eventToUpdate.VenueContactEmail;
                returnedEvent.VenueContactPhone = eventToUpdate.VenueContactPhone;

                db.Events.Update(returnedEvent);
                db.SaveChanges();

                return returnedEvent.MeetingID;
            }
        }

        public static void UpdateAttendance(EventGuest updatedGuest)
        {
            if (updatedGuest == null)
                throw new ArgumentNullException(nameof(updatedGuest));

            using (var db = new EventManagerDBContext())
            {
                var query = from guest in db.EventGuests
                            where guest.MeetingGuestID == updatedGuest.ID
                            select guest;
                var returnedGuest = query.First();

                if (returnedGuest == null)
                    throw new IndexOutOfRangeException($"Event index {updatedGuest.ID} not found");

                returnedGuest.GuestStatus = updatedGuest.GuestStatus;

                db.EventGuests.Update(returnedGuest);
                db.SaveChanges();
                //    SaveDatabase();
            }
        }
        #endregion

        #region Manage existing events
        /// <summary>
        /// Delete event
        /// </summary>
        public static void DeleteEvent(BaseEventItemModel eventToDelete)
        {
            if (eventToDelete == null) throw new ArgumentNullException();
            using (var db = new EventManagerDBContext())
            {
                var query = from event1 in db.Events
                            where event1.MeetingID == eventToDelete.ID
                            select event1;
                var result = query.FirstOrDefault();
                if (result != null)
                {
                    db.Events.Remove(result);

                    // Delete attendance
                    var queryGuest = from event1 in db.EventGuests
                                     where event1.MeetingID == eventToDelete.ID
                                     select event1;
                    db.EventGuests.RemoveRange(queryGuest);

                    db.SaveChanges();
                }
            }
        }
        #endregion

        public static IEnumerable<int> GrabDates(BaseEventItemModel item, int month, int year)
        {
            var date = new DateTime(item.EventBegin.Year, item.EventBegin.Month, item.EventBegin.Day);

            while (true)
            {
                if (date.Month == month && date.Year == year)
                {
                    yield return date.Day;
                }

                if (date.Month == item.EventEnd.Month && date.Day == item.EventEnd.Day)
                {
                    break;
                }

                date = date.AddDays(1);
            }
        }

        public static int GetCalendarMonthOffset(int month, int year)
        {
            var monthCal = new DateTime(year, month, 1);
            var firstDayIndex = (int)monthCal.DayOfWeek + 1;
            firstDayIndex = firstDayIndex == 7 ? 0 : firstDayIndex;
            firstDayIndex--;

            return firstDayIndex;
        }

        /// <summary>
        /// Get calendar offset for current month
        /// </summary>
        public static int GetCalendarMonthOffset()
        {
            var monthCal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var firstDayIndex = (int)monthCal.DayOfWeek + 1;
            firstDayIndex = firstDayIndex == 7 ? 0 : firstDayIndex;
            firstDayIndex--;

            return firstDayIndex;
        }

        #region BD Management
        /// <summary>
        /// Make sure database is created
        /// </summary>
        public static async void EnsureDbCreated(bool generateRandomData)
        {
            try
            {
                Random rand = new Random();
                using (var db = new EventManagerDBContext())
                {
                    // Add test data if testing
                    if (await db.Database.EnsureCreatedAsync() && generateRandomData)
                    {
                        // Generate test contacts
                        var jsonObject = JsonObject.Parse(await LoadData());

                        // Create events
                        foreach (JsonValue groupValue in jsonObject.GetNamedArray("ContactGroups", null))
                        {
                            // Create contact group
                            var groupObject = groupValue.GetObject();

                            // Add contacts
                            foreach (JsonValue itemValue in groupObject.GetNamedArray("Items", null))
                            {
                                var itemObject = itemValue.GetObject();

                                // Get image path
                                var imageString = itemObject.GetNamedString("ImagePath", string.Empty);
                                if (string.IsNullOrWhiteSpace(imageString))
                                    imageString = DefaultContactImagePath;

                                db.Contacts.Add(new Contact
                                {
                                    EmailAddress = itemObject.GetNamedString("EmailAddress", string.Empty),
                                    EmailAddress2 = itemObject.GetNamedString("EmailAddress2", string.Empty),
                                    FirstName = itemObject.GetNamedString("FirstName", string.Empty),
                                    LastName = itemObject.GetNamedString("LastName", string.Empty),
                                    PhoneNumber = itemObject.GetNamedString("PhoneNumber", string.Empty),
                                    PhoneNumber2 = itemObject.GetNamedString("PhoneNumber2", string.Empty),
                                    ImagePath = itemObject.GetNamedString("ImagePath", DefaultContactImagePath)
                                });
                            }
                        }
                        db.SaveChanges();

                        // 
                        foreach (JsonValue groupValue in jsonObject.GetNamedArray("EventsGroups", null))
                        {
                            // Create contact group
                            var groupObject = groupValue.GetObject();

                            var monthOfset = int.Parse(groupObject.GetNamedString("MonthOfset"));

                            // Add contacts
                            foreach (JsonValue itemValue in groupObject.GetNamedArray("Items", null))
                            {
                                var itemObject = itemValue.GetObject();

                                // Get image path
                                var imageString = itemObject.GetNamedString("ImagePath", string.Empty);
                                if (string.IsNullOrWhiteSpace(imageString))
                                    imageString = DefaultContactImagePath;

                                string startDate =
                                    (DateTime.Now.Month + monthOfset).ToString() +
                                    "-" + itemObject.GetNamedString("EventBeginDay", string.Empty) +
                                    "-" + (DateTime.Now.Year).ToString();

                                string endDate =
                                    (DateTime.Now.Month + monthOfset).ToString() +
                                    "-" + itemObject.GetNamedString("EventEndDay", string.Empty) +
                                    "-" + (DateTime.Now.Year).ToString();

                                var newEvent = new Meeting
                                {
                                    EventTitle = itemObject.GetNamedString("EventTitle", string.Empty),
                                    Venue = itemObject.GetNamedString("Venue", string.Empty),
                                    Description = itemObject.GetNamedString("Description", string.Empty),
                                    EventStart = DateTime.Parse(startDate),
                                    EventEnd = DateTime.Parse(endDate),
                                    ImagePath = itemObject.GetNamedString("ImagePath", DefaultEventImagePath),
                                    VenueContactEmail = itemObject.GetNamedString("VenueContactEmail", string.Empty),
                                    VenueContactPhone = itemObject.GetNamedString("VenueContactPhone", string.Empty),
                                };

                                db.Events.Add(newEvent);
                                AddRandomGuestList(db, newEvent);
                            }
                        }

                        db.SaveChanges();
                    }
                }
            }
            catch
            {
                // System.Diagnostics.Tracing.
            }
        }

        private static async Task<string> LoadData()
        {
            try
            {
                var dataUri = new Uri("ms-appx:///Resources/DesignData/DesignData.json");
                var file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
                using (await file.OpenAsync(FileAccessMode.Read))
                {
                    return await FileIO.ReadTextAsync(file);
                }
            }
            catch
            {
                return await LoadData();
            }
        }

        private static void AddRandomGuestList(EventManagerDBContext db, Meeting newEvent)
        {
            Random rand = new Random();

            // Get all contacts
            var contactList = new List<Contact>();
            contactList.AddRange(from c in db.Contacts select c);

            // Randomly select contacts for test event
            int noOfGuests = rand.Next(0, contactList.Count / 2);
            for (int i = 0; i < noOfGuests; i++)
            {
                int guestNo = rand.Next(0, contactList.Count - 1);
                var contact = contactList[guestNo];

                var guest =
                    new MeetingGuest
                    {
                        Contact = contact,
                        GuestStatus = GustArrivalStatus.Unknown,
                        Meeting = newEvent,
                    };

                db.EventGuests.Add(guest);
                contactList.RemoveAt(guestNo);
            }

            db.SaveChanges();
        }
        #endregion
    }
}
