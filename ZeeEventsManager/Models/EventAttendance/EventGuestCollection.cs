//------------------------------------------------------------
// <copyright file="EventGuestCollectionModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System.Collections;
    using System.Collections.Specialized;
    using System;

    /// <summary>
    /// Collection of guests attending an event
    /// </summary>
    public class EventGuestCollectionModel :
        CollectionModelBase<EventGuest, ContactCategoryModel>
    {
        Hashtable contactHashItems;
        Hashtable categoryHashItems;

        public EventGuestCollectionModel()
        {
            contactHashItems = new Hashtable();
            categoryHashItems = new Hashtable();

            Items.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (EventGuest item in e.NewItems)
                    {
                        if (item.ContactID > 0)
                        {
                            contactHashItems.Add(item.ContactID, item);
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Move)
                {
                    foreach (EventGuest item in e.NewItems)
                    {
                        if (item.ContactID > 0)
                        {
                            contactHashItems.Remove(item.ContactID);
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (EventGuest item in e.OldItems)
                    {
                        if (item.ContactID > 0)
                        {
                            contactHashItems.Remove(item.ContactID);
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Replace)
                {
                    foreach (EventGuest item in e.NewItems)
                    {
                        if (item.ContactID > 0)
                        {
                            contactHashItems.Remove(item.ContactID);
                        }
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    contactHashItems.Clear();
                }
            };
        }

        /// <summary>
        /// Get category by name
        /// </summary>
        public override ContactCategoryModel GetCategory(string catName)
        {
            if (categoryHashItems.ContainsKey(catName))
            {
                return categoryHashItems[catName] as ContactCategoryModel;
            }
            else
            {
                var newContact = new ContactCategoryModel { Title = catName };
                categoryHashItems.Add(catName, newContact);

                return newContact;
            }
        }

        public void RemoveContact(int contactId)
        {
            if (contactHashItems.ContainsKey(contactId))
            {
                var item = (EventGuest)contactHashItems[contactId];

                Items.Remove(item);
            }
        }

        /// <summary>
        /// Get contact by Contact ID
        /// </summary>
        public bool ContainsContact(int contactId)
        {
            if (contactHashItems.ContainsKey(contactId))
                return true;
            else
                return false;
        }

        public EventGuest GetEventGuest(int contactId)
        {
            if (contactHashItems.ContainsKey(contactId))
                return contactHashItems[contactId] as EventGuest;
            else
                return null;
        }
    }
}