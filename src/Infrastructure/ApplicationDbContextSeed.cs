using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Entities.ReservationAggregate;
using System;

namespace Infrastructure
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext db, UserManager<HotelPersonal> userManager)
        {
            long hotelId = await EnsureHotel(db);
            await EnsureRooms(db, hotelId);
            await EnsureReservations(db, hotelId);
            await EnsureUser(userManager, "test@domain.com", "P@ssword1", hotelId);
        }

        private static async Task EnsureUser(UserManager<HotelPersonal> userManager, string userName, string password, long hotelId)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new HotelPersonal { UserName = userName, Email = userName, HotelId = hotelId };
                await userManager.CreateAsync(user, password);
            }
        }

        private static async Task<long> EnsureHotel(ApplicationDbContext db)
        {
            if (db.Hotels.Any())
            {
                return db.Hotels.Select(h => h.Id).First();
            }
            else
            {
                var address = new Address("Great George St", "London", "UK", "E1 0AE");
                var hotel = new Hotel("Marigold Hotel", address);
                db.Hotels.Add(hotel);
                await db.SaveChangesAsync();
                return hotel.Id;
            }
        }

        private static async Task EnsureRooms(ApplicationDbContext db, long hotelId)
        {
            Hotel hotel = await db.Hotels.FindAsync(hotelId);
            if (hotel != null && !hotel.Rooms.Any())
            {
                for (int i = 0; i < 5; i++)
                {
                    hotel.AddRoom(new SingleRoom(hotelId, 0, 35));
                    hotel.AddRoom(new SingleRoom(hotelId, 1, 35));

                    hotel.AddRoom(new StandardRoom(hotelId, 0, 45));
                    hotel.AddRoom(new StandardRoom(hotelId, 1, 45));

                    hotel.AddRoom(new SuiteRoom(hotelId, 0, 60));
                    hotel.AddRoom(new SuiteRoom(hotelId, 1, 60));
                }

                await db.SaveChangesAsync();
            }
        }

        private static async Task EnsureReservations(ApplicationDbContext db, long hotelId)
        {
            Hotel hotel = await db.Hotels.FindAsync(hotelId);
            if (hotel != null && !db.Reservations.Any() && hotel.Rooms.Any())
            {
                var singleRoom = hotel.Rooms.Where(r => r.Type == "Single").FirstOrDefault();
                if (singleRoom != null)
                {
                    var customer = new Customer("Michael", "Smith", "00444567123");
                    db.Reservations.Add(new Reservation(singleRoom.Id, customer, DateTime.Today, DateTime.Today.AddDays(1)));
                }
                
                var standardRoom = hotel.Rooms.Where(r => r.Type == "Standard").FirstOrDefault();
                if (standardRoom != null)
                {
                    var customer = new Customer("Vanesa", "Jackson", "00442567188");
                    db.Reservations.Add(new Reservation(standardRoom.Id, customer, DateTime.Today, DateTime.Today.AddDays(7)));
                }

                await db.SaveChangesAsync();
                var tt = db.Reservations.ToList();
            }
        }

        //private static IEnumerable<RoomType> GetDefaultRoomTypes()
        //{
        //    return new List<RoomType>
        //    {
        //        new RoomType("Single"),
        //        new RoomType("Camera"),
        //        new RoomType("Laptops")
        //    };
        //}
    }
}
