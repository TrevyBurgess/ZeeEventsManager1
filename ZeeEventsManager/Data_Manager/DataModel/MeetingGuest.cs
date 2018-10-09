//------------------------------------------------------------
// <copyright file="MeetingGuest.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using System.ComponentModel.DataAnnotations;

    public class MeetingGuest
    {
        [Key]
        public int MeetingGuestID { get; set; }

        public GustArrivalStatus GuestStatus { get; set; }
        
        public int ContactID { get; set; }
        public virtual Contact Contact { get; set; }

        public int MeetingID { get; set; }
        public virtual Meeting Meeting { get; set; }
    }
}
