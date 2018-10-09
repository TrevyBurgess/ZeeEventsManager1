//------------------------------------------------------------
// <copyright file="ContactCollectionModel.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Common.WPF;
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// Collection of all contacts
    /// </summary>
    public class ContactCollectionModel : CollectionModelBase<ContactItemModel, ContactCategoryModel>
    {
        public override ContactCategoryModel GetCategory(string catName)
        {
            foreach (var contact in Categories)
            {
                if (contact.Title.Equals(catName))
                    return contact;
            }

            var newContact = new ContactCategoryModel { Title = catName };
            Categories.Add(newContact);

            return newContact;
        }

        /// <summary>
        /// Add new contact or update existing contact, and save to file
        /// </summary>
        public void AddOrUpdate(NewContactItemModel contact, bool userNameInStandardFormat)
        {
            AddOrUpdate(ConvertModel(contact), userNameInStandardFormat);
        }

        /// <summary>
        /// Add new contact or update existing contact, and save to file
        /// </summary>
        public void AddOrUpdate(ContactItemModel contact, bool userNameInStandardFormat)
        {
            var catName = userNameInStandardFormat
                ? contact.LastName[0].ToString().ToUpper() : contact.FirstName[0].ToString().ToUpper();

            if (contact.ID == -1)
            {
                // Update contact
                var id = SqLiteManager.AddOrUpdateContact(contact);
                contact.ID = id;
                contact.Category = GetCategory(catName);
                Items.Add(contact);

                RaiseCollectionChange(
                    contact,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
            else
            {
                foreach (var item in Items)
                {
                    if (item.ID == contact.ID)
                    {
                        var newCatName = userNameInStandardFormat
                            ? contact.LastName[0].ToString().ToUpper() : contact.FirstName[0].ToString().ToUpper();

                        item.EmailAddress = contact.EmailAddress;
                        item.EmailAddress2 = contact.EmailAddress2;
                        item.FirstName = contact.FirstName;
                        item.LastName = contact.LastName;
                        item.PhoneNumber = contact.PhoneNumber;
                        item.PhoneNumber2 = contact.PhoneNumber2;
                        item.Category = GetCategory(newCatName);
                        item.ImagePath = contact.ImagePath;

                        RaiseCollectionChange(
                            contact,
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        SqLiteManager.AddOrUpdateContact(contact);

                        return;
                    }
                }

                throw new IndexOutOfRangeException($"Contact ID {contact.ID} not found");
            }
        }

        internal void UpdateOrdering(bool userNameInStandardFormat)
        {
            Categories.Clear();

            foreach (var item in Items)
            {
                string newCatName;
                if (userNameInStandardFormat)
                {
                    newCatName = item.LastName[0].ToString().ToUpper();
                    item.PageTitle = $"{item.LastName}, {item.FirstName}";
                }
                else
                {
                    newCatName=item.FirstName[0].ToString().ToUpper();
                    item.PageTitle = $"{item.FirstName} {item.LastName}";
                }

                item.Category = GetCategory(newCatName);
            }

            RaiseCollectionChange(null,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public static ContactItemModel ConvertModel(NewContactItemModel contact)
        {
            return new ContactItemModel(contact.AppViewModel)
            {
                ID = contact.ID,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                EmailAddress = contact.EmailAddress,
                EmailAddress2 = contact.EmailAddress2,
                PhoneNumber = contact.PhoneNumber,
                PhoneNumber2 = contact.PhoneNumber2,
                ImagePath = contact.ImagePath
            };
        }
    }
}
