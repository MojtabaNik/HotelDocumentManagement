using System.Data.Entity;
using EntityModel;

namespace DataAccessLayer
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        public DbSet<Post> Posts { get; set; }

        public DbSet<ReceivedLetter> ReceiveLetters { get; set; }
        public DbSet<ReceiveLetterFile> ReceiveLetterFiles { get; set; }
        public DbSet<ReceiveAppendage> ReceiveSendAppendages { get; set; }
        public DbSet<TempReceivedLetter> TempReceiveLetters { get; set; }
        public DbSet<TempReceiveLetterFile> TempReceiveLetterFiles { get; set; }
        public DbSet<TempReceiveAppendage> TempReceiveSendAppendages { get; set; }




        public DbSet<SendLetter> SendLetters { get; set; }
        public DbSet<SendLetterFile> SendLetterFiles { get; set; }
        public DbSet<SendAppendage> SendAppendages { get; set; }

        public DbSet<TempSendLetter> TempSenderLetters { get; set; }
        public DbSet<TempSendLetterFile> TempSendLetterFiles { get; set; }
        public DbSet<TempSendAppendage> TempSendAppendages { get; set; }



        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Department> Departements { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<Person> Persons { get; set; }

        public DbSet<SystemLog> SystemLogs { get; set; }
        public DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TempReceivedLetter>()
                .HasRequired(t => t.ReceivedLetter)
                .WithMany(r => r.TempReceiveLetters)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TempSendLetter>()
                .HasRequired(t => t.SendLetter)
                .WithMany(r => r.TempSendLetters)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

            //Set Parents Relations
            modelBuilder.Entity<ReceivedLetter>()
                .HasOptional(t => t.ParentSendLetterOut)
                .WithMany(r => r.ParentSendLetterOut)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SendLetter>()
              .HasOptional(t => t.ParentReceivedLetterOut)
              .WithMany(r => r.ParentReceivedLetterOut)
              .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);

        }
    }
}
