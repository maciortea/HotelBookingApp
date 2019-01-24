using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Entities.ReservationAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<HotelPersonal>
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Hotel>(ConfigureHotel);
            builder.Entity<Reservation>(ConfigureReservation);
        }

        private void ConfigureHotel(EntityTypeBuilder<Hotel> builder)
        {
            var navigation = builder.Metadata.FindNavigation(nameof(Hotel.Rooms));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(h => h.Address);
        }

        private void ConfigureReservation(EntityTypeBuilder<Reservation> builder)
        {
            builder.OwnsOne(r => r.Customer);
        }
    }
}
