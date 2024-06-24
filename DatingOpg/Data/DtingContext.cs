using DatingOpg.Models;
using Microsoft.EntityFrameworkCore;
namespace DatingOpg.Data
{
    public class DtingContext : DbContext
    {
        public DtingContext(DbContextOptions<DtingContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Chat> Chats { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the primary key of City not to be an identity column
            modelBuilder.Entity<Like>()
                .HasOne(l => l.Sender)
                .WithMany(a => a.SentLikes)
                .HasForeignKey(l => l.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Receiver)
                .WithMany(p => p.ReceivedLikes)
                .HasForeignKey(l => l.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Account)
                .WithOne(a => a.Profile)
                .HasForeignKey<Profile>(p => p.AccountId);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Sender)
                .WithMany(a => a.SentChats)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Receiver)
                .WithMany()
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.ReceiverProfile)
                .WithMany(p => p.ReceivedChats)
                .HasForeignKey(c => c.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
