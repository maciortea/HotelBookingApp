using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Entities.ReservationAggregate;
using System;
using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext db, UserManager<HotelPersonal> userManager)
        {
            long hotelId = await EnsureHotelAsync(db);
            await EnsureRoomsAsync(db, hotelId);
            await AddRoomItemsToHotelAsync(db, hotelId);
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

                return hotel.Id;
            }
        }

        private static async Task EnsureRoomsAsync(ApplicationDbContext db, long hotelId)
        {
            if (!db.Rooms.Any())
            {
                await db.Rooms.AddAsync(new SingleRoom(hotelId, Euros.Of(35)));
                await db.Rooms.AddAsync(new StandardRoom(hotelId, Euros.Of(45)));

                var suiteRoome = new SuiteRoom(hotelId, Euros.Of(60));
                await db.Rooms.AddAsync(suiteRoome);
                await db.SaveChangesAsync();

                suiteRoome.AddFacility(FacilityFactory.CreateRoomFacility("Minibar", Euros.Of(5), suiteRoome.Id));
                await db.SaveChangesAsync();
            }
        }

        private static async Task AddRoomItemsToHotelAsync(ApplicationDbContext db, long hotelId)
        {
            Hotel hotel = await db.Hotels.FindAsync(hotelId);
            var rooms = await db.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();
            if (hotel != null && !hotel.RoomItems.Any() && rooms.Count > 0)
            {
                long singleRoomId = rooms.Where(r => r.Type == "Single").Select(r => r.Id).FirstOrDefault();
                long standardRoomId = rooms.Where(r => r.Type == "Standard").Select(r => r.Id).FirstOrDefault();
                long suiteRoomId = rooms.Where(r => r.Type == "Suite").Select(r => r.Id).FirstOrDefault();

                if (singleRoomId > 0 && standardRoomId > 0 && suiteRoomId > 0)
                {
                    int roomNumber = 1;
                    for (int i = 0; i < 5; i++)
                    {
                        hotel.AddRoomItem(new RoomItem(singleRoomId, 0, roomNumber));
                        roomNumber++;
                        hotel.AddRoomItem(new RoomItem(singleRoomId, 1, roomNumber));
                        roomNumber++;
                        hotel.AddRoomItem(new RoomItem(standardRoomId, 0, roomNumber));
                        roomNumber++;
                        hotel.AddRoomItem(new RoomItem(standardRoomId, 1, roomNumber));
                        roomNumber++;
                        hotel.AddRoomItem(new RoomItem(suiteRoomId, 0, roomNumber));
                        roomNumber++;
                        hotel.AddRoomItem(new RoomItem(suiteRoomId, 1, roomNumber));
                        roomNumber++;
                    }

                    await db.SaveChangesAsync();
                }
            }
        }

        private static async Task EnsureReservationsAsync(ApplicationDbContext db, long hotelId)
        {
            Hotel hotel = await db.Hotels.FindAsync(hotelId);
            if (hotel != null && !db.Reservations.Any() && hotel.RoomItems.Any())
            {
                var singleRoomItemId = hotel.RoomItems.Where(r => r.Room.Type == "Single").Select(r => r.RoomId).FirstOrDefault();
                if (singleRoomItemId > 0)
                {
                    var customer = new Customer("Michael", "Smith", "00444567123");
                    db.Reservations.Add(new Reservation(singleRoomItemId, customer, DateTime.Today.AddDays(1), DateTime.Today.AddDays(3)));
                }

                var standardRoomItemId = hotel.RoomItems.Where(r => r.Room.Type == "Standard").Select(r => r.RoomId).FirstOrDefault();
                if (standardRoomItemId > 0)
                {
                    var customer = new Customer("Vanesa", "Jackson", "00442567188");
                    db.Reservations.Add(new Reservation(standardRoomItemId, customer, DateTime.Today, DateTime.Today.AddDays(7)));
                }

                var suiteRoomItemId = hotel.RoomItems.Where(r => r.Room.Type == "Suite").Select(r => r.RoomId).FirstOrDefault();
                if (suiteRoomItemId > 0)
                {
                    var customer = new Customer("Max", "Donovan", "00442567188");
                    db.Reservations.Add(new Reservation(suiteRoomItemId, customer, DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1)));
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
