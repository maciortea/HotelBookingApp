using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : EfRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<Reservation> GetFullByIdAsync(long id)
        {
            return await _db.Reservations
                .Include(r => r.Facilities).ThenInclude(f => f.HotelFacility)
                .Include(r => r.RoomItem.Room)
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<Reservation>> GetAllByHotelIdAsync(long hotelId)
        {
            return await _db.Reservations
                .Include(r => r.RoomItem.Room)
                .Where(r => r.RoomItem.Room.HotelId == hotelId && !r.CheckedOut && !r.Canceled)
                .ToListAsync();
        }

        public async Task<long[]> GetIdsByHotelIdAndPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            return await _db.Reservations
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
