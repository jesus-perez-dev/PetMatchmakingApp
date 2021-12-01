using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Connection>().HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<Connection>().HasOne(s => s.SourceUser).WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId).OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Connection>().HasOne(s => s.LikedUser).WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId).OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Message>().HasOne(m => m.Receiver).WithMany(u => u.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Message>().HasOne(m => m.Sender).WithMany(u => u.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}