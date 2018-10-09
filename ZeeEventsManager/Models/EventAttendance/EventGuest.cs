//------------------------------------------------------------
// <copyright file="EventGuest.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using System;
    using Common.WPF;
    using Windows.UI.Xaml.Media;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Event guest
    /// </summary>
    public class EventGuest : ViewModelBase, IItemModel<ContactCategoryModel>
    {
        /// <summary>
        /// MeetingGuestID
        /// </summary>
        public int ID
        {
            get
            {
                return GetState(-1);
            }
            set
            {
                SetState(value);
            }
        }

        public int ContactID
        {
            get
            {
                return GetState(-1);
            }
            set
            {
                SetState(value);
            }
        }

        /// <summary>
        /// Tracks if a guest arrived
        /// </summary>
        public GustArrivalStatus GuestStatus
        {
            get
            {
                return GetState(GustArrivalStatus.Unknown);
            }
            set
            {
                SetState(value);
            }
        }

        public ContactItemModel Guest
        {
            get
            {
                return GetState<ContactItemModel>();
            }
            set
            {
                SetState(value);
            }
        }

        [NotMapped]
        public ContactCategoryModel Category
        {
            get
            {
                return Guest.Category;
            }
        }

        [NotMapped]
        public string PageTitle
        {
            get
            {
                return Guest.PageTitle;
            }
        }

        [NotMapped]
        public ImageSource Image
        {
            get
            {
                return Guest.Image;
            }
        }
    }
}
