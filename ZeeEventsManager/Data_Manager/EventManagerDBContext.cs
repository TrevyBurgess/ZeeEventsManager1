//------------------------------------------------------------
// <copyright file="EventManagerDBContext.cs" company="TrevyBurgess" >
// Free for use, modification and distribution
// </copyright>
// <Author>
// Trevy Burgess
// </Author>
//------------------------------------------------------------
namespace CyberFeedForward.WUP.Social.ZeeEventsManager
{
    using Microsoft.Data.Entity;
    using Microsoft.Data.Sqlite;

    public class EventManagerDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var csb = new SqliteConnectionStringBuilder($"Filename={SqLiteManager.SqLiteDatabasePath}");
            optionsBuilder.UseSqlite(csb.ToString());
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<MeetingGuest> EventGuests { get; set; }
        public DbSet<Meeting> Events { get; set; }
    }
}
