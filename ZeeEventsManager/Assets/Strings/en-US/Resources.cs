//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------------------
// <auto-generatedInfo>
// 	This code was generated by ResW File Code Generator (http://reswcodegen.codeplex.com)
// 	ResW File Code Generator was written by Christian Resma Helle
// 	and is under GNU General Public License version 2 (GPLv2)
// 
// 	This code contains a helper class exposing property representations
// 	of the string resources defined in the specified .ResW file
// 
// 	Generated: 05/18/2016 19:09:25
// </auto-generatedInfo>
// --------------------------------------------------------------------------------------------------
namespace Q
{
    using Windows.ApplicationModel.Resources;
    
    
    public partial class Resources
    {
        
        private static ResourceLoader resourceLoader;
        
        static Resources()
        {
            string executingAssemblyName;
            executingAssemblyName = Windows.UI.Xaml.Application.Current.GetType().AssemblyQualifiedName;
            string[] executingAssemblySplit;
            executingAssemblySplit = executingAssemblyName.Split(',');
            executingAssemblyName = executingAssemblySplit[1];
            string currentAssemblyName;
            currentAssemblyName = typeof(Resources).AssemblyQualifiedName;
            string[] currentAssemblySplit;
            currentAssemblySplit = currentAssemblyName.Split(',');
            currentAssemblyName = currentAssemblySplit[1];
            if (executingAssemblyName.Equals(currentAssemblyName))
            {
                resourceLoader = ResourceLoader.GetForCurrentView("Resources");
            }
            else
            {
                resourceLoader = ResourceLoader.GetForCurrentView(currentAssemblyName + "/Resources");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "About"
        /// </summary>
        public static string About_PageTitle
        {
            get
            {
                return resourceLoader.GetString("About_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "CREATE NEW EVENT"
        /// </summary>
        public static string AppLevel_CreateNewEvent
        {
            get
            {
                return resourceLoader.GetString("AppLevel_CreateNewEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Current Event"
        /// </summary>
        public static string AppLevel_CurrentEvent
        {
            get
            {
                return resourceLoader.GetString("AppLevel_CurrentEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Latest Event"
        /// </summary>
        public static string AppLevel_LatestEvent
        {
            get
            {
                return resourceLoader.GetString("AppLevel_LatestEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "My Contacts"
        /// </summary>
        public static string AppLevel_MyContacts
        {
            get
            {
                return resourceLoader.GetString("AppLevel_MyContacts");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add New Event"
        /// </summary>
        public static string Calendar_AddNewEvent
        {
            get
            {
                return resourceLoader.GetString("Calendar_AddNewEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Calendar"
        /// </summary>
        public static string Calendar_PageTitle
        {
            get
            {
                return resourceLoader.GetString("Calendar_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add new Contact"
        /// </summary>
        public static string ContactItem_PageTitle
        {
            get
            {
                return resourceLoader.GetString("ContactItem_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add new Contact"
        /// </summary>
        public static string ContactNew_PageTitle
        {
            get
            {
                return resourceLoader.GetString("ContactNew_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Changes will be lost"
        /// </summary>
        public static string Contact_CancelConfirmDialog_Message
        {
            get
            {
                return resourceLoader.GetString("Contact_CancelConfirmDialog_Message");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Confirm cancel changes"
        /// </summary>
        public static string Contact_CancelConfirmDialog_Title
        {
            get
            {
                return resourceLoader.GetString("Contact_CancelConfirmDialog_Title");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Discard"
        /// </summary>
        public static string Contact_Discard
        {
            get
            {
                return resourceLoader.GetString("Contact_Discard");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Edit"
        /// </summary>
        public static string Contact_Edit
        {
            get
            {
                return resourceLoader.GetString("Contact_Edit");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "<First Name>"
        /// </summary>
        public static string Contact_Placeholder_FirstName
        {
            get
            {
                return resourceLoader.GetString("Contact_Placeholder_FirstName");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "<Last Name>"
        /// </summary>
        public static string Contact_Placeholder_LastName
        {
            get
            {
                return resourceLoader.GetString("Contact_Placeholder_LastName");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Return to My Contacts"
        /// </summary>
        public static string Contact_ReturnToMyContacts
        {
            get
            {
                return resourceLoader.GetString("Contact_ReturnToMyContacts");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Save"
        /// </summary>
        public static string Contact_Save
        {
            get
            {
                return resourceLoader.GetString("Contact_Save");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "First Name, Last Name can't be blank"
        /// </summary>
        public static string Contact_SaveConfirmDialog_Message
        {
            get
            {
                return resourceLoader.GetString("Contact_SaveConfirmDialog_Message");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Save Command"
        /// </summary>
        public static string Contact_SaveConfirmDialog_Title
        {
            get
            {
                return resourceLoader.GetString("Contact_SaveConfirmDialog_Title");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Attendance"
        /// </summary>
        public static string EventAttendance_PageTitle
        {
            get
            {
                return resourceLoader.GetString("EventAttendance_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Accept Changes"
        /// </summary>
        public static string Event_AcceptChanges
        {
            get
            {
                return resourceLoader.GetString("Event_AcceptChanges");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Changes will be lost"
        /// </summary>
        public static string Event_CancelConfirmDialog_Message
        {
            get
            {
                return resourceLoader.GetString("Event_CancelConfirmDialog_Message");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Confirm cancel changes"
        /// </summary>
        public static string Event_CancelConfirmDialog_Title
        {
            get
            {
                return resourceLoader.GetString("Event_CancelConfirmDialog_Title");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Are you sure you want to delete event"
        /// </summary>
        public static string Event_DeleteConfirmDialog_Message
        {
            get
            {
                return resourceLoader.GetString("Event_DeleteConfirmDialog_Message");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Delete Command"
        /// </summary>
        public static string Event_DeleteConfirmDialog_Title
        {
            get
            {
                return resourceLoader.GetString("Event_DeleteConfirmDialog_Title");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Delete Event"
        /// </summary>
        public static string Event_DeleteEvent
        {
            get
            {
                return resourceLoader.GetString("Event_DeleteEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Discard Changes"
        /// </summary>
        public static string Event_DiscardChanges
        {
            get
            {
                return resourceLoader.GetString("Event_DiscardChanges");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Edit Attendance"
        /// </summary>
        public static string Event_EditAttendance
        {
            get
            {
                return resourceLoader.GetString("Event_EditAttendance");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Edit Event"
        /// </summary>
        public static string Event_EditEvent
        {
            get
            {
                return resourceLoader.GetString("Event_EditEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "<Set Title>"
        /// </summary>
        public static string Event_Placeholder_SetTitle
        {
            get
            {
                return resourceLoader.GetString("Event_Placeholder_SetTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "<Set Title> - (Event Past)"
        /// </summary>
        public static string Event_Placeholder_SetTitle_PastEvent
        {
            get
            {
                return resourceLoader.GetString("Event_Placeholder_SetTitle_PastEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "{0} - (Event Past)"
        /// </summary>
        public static string Event_Placeholder_Titled_PastEvent
        {
            get
            {
                return resourceLoader.GetString("Event_Placeholder_Titled_PastEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Return to Calendar"
        /// </summary>
        public static string Event_ReturnToCalendar
        {
            get
            {
                return resourceLoader.GetString("Event_ReturnToCalendar");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Return to Current Event"
        /// </summary>
        public static string Event_ReturnToCurrentEvent
        {
            get
            {
                return resourceLoader.GetString("Event_ReturnToCurrentEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Event title can't be blank"
        /// </summary>
        public static string Event_SaveConfirmDialog_Message
        {
            get
            {
                return resourceLoader.GetString("Event_SaveConfirmDialog_Message");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Save Command"
        /// </summary>
        public static string Event_SaveConfirmDialog_Title
        {
            get
            {
                return resourceLoader.GetString("Event_SaveConfirmDialog_Title");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Save Event"
        /// </summary>
        public static string Event_SaveEvent
        {
            get
            {
                return resourceLoader.GetString("Event_SaveEvent");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Cancel"
        /// </summary>
        public static string MessageDialog_Cancel
        {
            get
            {
                return resourceLoader.GetString("MessageDialog_Cancel");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Okay"
        /// </summary>
        public static string MessageDialog_Okay
        {
            get
            {
                return resourceLoader.GetString("MessageDialog_Okay");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Return to Event Guest List"
        /// </summary>
        public static string MyContacts_BackToEventList
        {
            get
            {
                return resourceLoader.GetString("MyContacts_BackToEventList");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "My Contacts"
        /// </summary>
        public static string MyContacts_PageTitle
        {
            get
            {
                return resourceLoader.GetString("MyContacts_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Back"
        /// </summary>
        public static string Navigation_Back
        {
            get
            {
                return resourceLoader.GetString("Navigation_Back");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Add new event"
        /// </summary>
        public static string NewEvent_PageTitle
        {
            get
            {
                return resourceLoader.GetString("NewEvent_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Settings"
        /// </summary>
        public static string Settings_PageTitle
        {
            get
            {
                return resourceLoader.GetString("Settings_PageTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "mailto:{0}?subject={1}&body={2}"
        /// </summary>
        public static string SmileyDialog_EmailMessage
        {
            get
            {
                return resourceLoader.GetString("SmileyDialog_EmailMessage");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "{0}-Feedback-{1}-Address: {2}"
        /// </summary>
        public static string SmileyDialog_EmailTitle
        {
            get
            {
                return resourceLoader.GetString("SmileyDialog_EmailTitle");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Frown"
        /// </summary>
        public static string SmileyDialog_Frown
        {
            get
            {
                return resourceLoader.GetString("SmileyDialog_Frown");
            }
        }
        
        /// <summary>
        /// Localized resource similar to "Smile"
        /// </summary>
        public static string SmileyDialog_Smile
        {
            get
            {
                return resourceLoader.GetString("SmileyDialog_Smile");
            }
        }
    }
}