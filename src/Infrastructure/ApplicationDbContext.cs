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
        public DbSet<RoomItem> RoomItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<HotelFacility> HotelFacilities { get; set; }
        public DbSet<RoomFacility> RoomFacilities { get; set; }
        public DbSet<ReservationFacility> ReservationFacilities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Hotel>(ConfigureHotel);
            builder.Entity<Reservation>(ConfigureReservation);
            builder.Entity<Room>(ConfigureRoom);
            builder.Entity<HotelFacility>(ConfigureHotelFacility);
            builder.Entity<RoomFacility>(ConfigureRoomFacility);
            builder.Entity<ReservationFacility>(ConfigureReservationFacilities);
        }

        private void ConfigureHotel(EntityTypeBuilder<Hotel> builder)
        {
            var roomsNavigation = builder.Metadata.FindNavigation(nameof(Hotel.RoomItems));
            roomsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var facilitiesNavigation = builder.Metadata.FindNavigation(nameof(Hotel.Facilities));
            facilitiesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(h => h.Address);
        }

        private void ConfigureReservation(EntityTypeBuilder<Reservation> builder)
        {
            builder.OwnsOne(r => r.Customer);
            builder.Ignore(r => r.NoOfNights);

            var facilitiesNavigation = builder.Metadata.FindNavigation(nameof(Reservation.Facilities));
            facilitiesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureRoom(EntityTypeBuilder<Room> builder)
        {
            var facilitiesNavigation = builder.Metadata.FindNavigation(nameof(Room.Facilities));
            facilitiesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(r => r.PricePerNight);
        }

        private void ConfigureHotelFacility(EntityTypeBuilder<HotelFacility> builder)
        {
            builder.OwnsOne(r => r.UnitPrice);
        }

        private void ConfigureRoomFacility(EntityTypeBuilder<RoomFacility> builder)
        {
            builder.OwnsOne(r => r.UnitPrice);
        }

        private void ConfigureReservationFacilities(EntityTypeBuilder<ReservationFacility> builder)
        {
            builder.HasOne(rf => rf.Reservation).WithMany(r => r.Facilities).HasForeignKey(rf => rf.ReservationId);
            builder.HasOne(rf => rf.HotelFacility).WithMany().HasForeignKey(rf => rf.HotelFacilityId);
        }
    }
}
