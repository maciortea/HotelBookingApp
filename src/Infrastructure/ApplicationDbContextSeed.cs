using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using ApplicationCore.Entities.HotelAggregate;

namespace Infrastructure
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            await EnsureUser(userManager, "test@domain.com", "P@ssword1");
            long hotelId = await EnsureHotel(db);
            await EnsureRooms(db, hotelId);
        }

        private static async Task EnsureUser(UserManager<IdentityUser> userManager, string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser { UserName = userName, Email = userName };
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
