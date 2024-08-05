using HotelBookingWebsite.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomTypeSideOption> RoomTypeSideOptions { get; set; }
        public DbSet<SideOption> SideOptions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //Thiet lap thuc the 
            builder.Entity<RoomTypeSideOption>()
               .HasKey(x => new { x.RoomTypeId, x.SideOptionId });
            builder.Entity<RoomType>()
                .HasMany(x => x.Rooms)
                .WithOne(r => r.RoomType)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Room>()
                .HasIndex(x => x.RoomNumber)
                .IsUnique();
            builder.Entity<Booking>()
                .HasOne(b => b.RoomType)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Payment>()
                .HasOne(b => b.Booking)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
