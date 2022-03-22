using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuestRoomMVC.Models;

namespace QuestRoomMVC.Data
{
    public class QuestRoomMVCContext : IdentityDbContext<User>
    {
        public QuestRoomMVCContext(DbContextOptions<QuestRoomMVCContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
