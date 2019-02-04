using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Entities.RoomTypeAggregate;
using ApplicationCore.Factories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext db, UserManager<HotelPersonal> userManager)
        {
            long hotelId = await EnsureHotelAsync(db);
            await EnsureReservationsAsync(db, hotelId);
            await EnsureUserAsync(userManager, "test@domain.com", "P@ssword1", hotelId);
        }

        private static async Task<long> EnsureHotelAsync(ApplicationDbContext db)
        {
            if (db.Hotels.Any())
            {
                return db.Hotels.Select(h => h.Id).First();
            }
            else
            {
                var address = new Address("Great George St", "London", "UK", "E1 0AE");
                var hotel = new Hotel("Marigold Hotel", address);

                await db.Hotels.AddAsync(hotel);
                await db.SaveChangesAsync();

                hotel.AddFacility(FacilityFactory.CreateFreeHotelFacility("WiFi", hotel.Id));
                hotel.AddFacility(FacilityFactory.CreateHotelFacility("Parking", Euros.Of(10), hotel.Id));
                hotel.AddFacility(FacilityFactory.CreateHotelFacility("Breakfast", Euros.Of(15), hotel.Id));

                db.Hotels.Update(hotel);
                await db.SaveChangesAsync();

                var singleRoom = new SingleRoom(hotel.Id, Euros.Of(35));
                var standardRoom = new StandardRoom(hotel.Id, Euros.Of(45));
                var suiteRoom = new SuiteRoom(hotel.Id, Euros.Of(60));

                await db.RoomTypes.AddAsync(singleRoom);
                await db.RoomTypes.AddAsync(standardRoom);
                await db.RoomTypes.AddAsync(suiteRoom);
                await db.SaveChangesAsync();

                suiteRoom.AddFacility(FacilityFactory.CreateRoomFacility("Minibar", Euros.Of(5), suiteRoom.Id));
                await db.SaveChangesAsync();

                int roomNumber = 1;
                for (int i = 0; i < 5; i++)
                {
                    hotel.AddRoom(new Room(singleRoom.Id, 0, roomNumber));
                    roomNumber++;
                    hotel.AddRoom(new Room(singleRoom.Id, 1, roomNumber));
                    roomNumber++;
                    hotel.AddRoom(new Room(standardRoom.Id, 0, roomNumber));
                    roomNumber++;
                    hotel.AddRoom(new Room(standardRoom.Id, 1, roomNumber));
                    roomNumber++;
                    hotel.AddRoom(new Room(suiteRoom.Id, 0, roomNumber));
                    roomNumber++;
                    hotel.AddRoom(new Room(suiteRoom.Id, 1, roomNumber));
                    roomNumber++;
                }

                await db.SaveChangesAsync();

                return hotel.Id;
            }
        }

        private static async Task EnsureReservationsAsync(ApplicationDbContext db, long hotelId)
        {
            Hotel hotel = await db.Hotels.FindAsync(hotelId);
            if (hotel != null && !db.Reservations.Any() && hotel.Rooms.Any())
            {
                var singleRoomId = hotel.Rooms.Where(r => r.RoomType.Type == "Single").Select(r => r.Id).FirstOrDefault();
                if (singleRoomId > 0)
                {
                    var customer = new Customer("Michael", "Smith", "00444567123");
                    db.Reservations.Add(new Reservation(singleRoomId, customer, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)));
                }

                var standardRoomId = hotel.Rooms.Where(r => r.RoomType.Type == "Standard").Select(r => r.Id).FirstOrDefault();
                if (standardRoomId > 0)
                {
                    var customer = new Customer("Vanesa", "Jackson", "00442567188");
                    db.Reservations.Add(new Reservation(standardRoomId, customer, DateTime.Today.AddDays(-1), DateTime.Today.AddDays(7)));
                }

                var suiteRoomId = hotel.Rooms.Where(r => r.RoomType.Type == "Suite").Select(r => r.Id).FirstOrDefault();
                if (suiteRoomId > 0)
                {
                    var customer = new Customer("Max", "Donovan", "00442567188");
                    db.Reservations.Add(new Reservation(suiteRoomId, customer, DateTime.Today.AddDays(-3), DateTime.Today.AddDays(-1)));
                }

                await db.SaveChangesAsync();
            }
        }

        private static async Task EnsureUserAsync(UserManager<HotelPersonal> userManager, string userName, string password, long hotelId)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new HotelPersonal { UserName = userName, Email = userName, HotelId = hotelId };
                await userManager.CreateAsync(user, password);
            }
            user = await userManager.FindByNameAsync(userName);
        }
    }
}
