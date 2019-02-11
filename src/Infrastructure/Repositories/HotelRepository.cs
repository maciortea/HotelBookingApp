using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class HotelRepository : EfRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext db)
            : base(db)
        {
        }
       
        public async Task<Dictionary<string, Tuple<int, decimal>>> GetRoomTypesToCountAndPrice(long hotelId)
        {
            return await _db.Hotels
                .Where(h => h.Id == hotelId)
                .SelectMany(h => h.Rooms)
                .GroupBy(r => new { r.RoomType.Type })
                .Select(r => new
                {
                    Type = r.Key,
                    Count = r.Count(),
                    PricePerNight = r.Select(x => x.RoomType.PricePerNight).FirstOrDefault()
                })
                .ToDictionaryAsync(k => k.Type.Type, v => Tuple.Create<int, decimal>(v.Count, v.PricePerNight));
        }
    }
}
