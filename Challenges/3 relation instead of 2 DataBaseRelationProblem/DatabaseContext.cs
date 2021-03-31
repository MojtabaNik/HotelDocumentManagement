using System.Data.Entity;
using EntityModel;

namespace DataAccessLayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
            SetDatabaseInitializer.Set();
        }

   
        public DbSet<SendLetter> SenderLetters { get; set; }

        public DbSet<TempSendLetter> TempSenderLetters { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TempReceivedLetter>()
                .HasRequired(t => t.ReceivedLetter)
                .WithMany(R => R.TempReceiveLetter)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TempSendLetter>()
                .HasRequired(t => t.SendLetter)
                .WithMany(R => R.TempSendLetter)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

        }
    }
}
