//------------------------------------------------------------
// <copyright file="Meeting.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------ 
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class Meeting
    {
        [Key]
        public int MeetingID { get; set; }
        
        public string EventTitle { get; set; }

        public string Venue { get; set; }

        public string Description { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventEnd { get; set; }

        public string VenueContactEmail { get; set; }

        public string VenueContactPhone { get; set; }
        
        ////////////////////
        // Store Image here
        ////////////////////

        public string ImagePath { get; set; }
    }
}
