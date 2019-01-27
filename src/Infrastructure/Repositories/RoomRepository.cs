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

        public async Task<List<RoomItem>> GetAvailableByHotelIdAndPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            var reservedRoomItemIds = await GetReservedRoomItemIdsByHotelIdAndPeriodAsync(hotelId, checkinDate, checkoutDate);
            return await _db.RoomItems
                .Include(r => r.Room)
                .Where(r => r.Room.HotelId == hotelId && !reservedRoomItemIds.Contains(r.Id))
                .OrderBy(r => r.Number)
                .ToListAsync();
        }

        private async Task<long[]> GetReservedRoomItemIdsByHotelIdAndPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            return await _db.Reservations
                .Include(r => r.RoomItem)
                .Where(r =>
                    r.RoomItem.Room.HotelId == hotelId &&
                    checkinDate <= r.CheckinDate &&
                    checkoutDate <= r.CheckoutDate &&
                    !r.CheckedOut &&
                    !r.Canceled)
                .Select(r => r.RoomItemId)
                .ToArrayAsync();
        }
    }
}
