using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoomRepository : Repository, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<Room>> GetAvailableByHotelIdAndPeriod(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            var reservedRoomIds = await GetReservedRoomIdsByHotelIdAndPeriod(hotelId, checkinDate, checkoutDate);
            return await _db.Rooms.Where(r => !reservedRoomIds.Contains(r.Id)).OrderBy(r => r.Floor).ToListAsync();
        }

        private async Task<long[]> GetReservedRoomIdsByHotelIdAndPeriod(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            return await _db.Reservations
                .Include(r => r.Room)
                .Where(r => r.Room.HotelId == hotelId && checkinDate <= r.CheckinDate && checkoutDate <= r.CheckoutDate)
                .Select(r => r.RoomId)
                .ToArrayAsync();
        }
    }
}
