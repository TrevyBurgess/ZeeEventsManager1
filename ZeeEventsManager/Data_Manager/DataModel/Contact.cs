//------------------------------------------------------------
// <copyright file="Contact.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using System.ComponentModel.DataAnnotations;

    public class Contact
    {
        [Key]
        public int ContactID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumber2 { get; set; }

        public string EmailAddress { get; set; }

        public string EmailAddress2 { get; set; }

        ////////////////////
        // Store Image here
        ////////////////////

        public string ImagePath { get; set; }
    }
}
