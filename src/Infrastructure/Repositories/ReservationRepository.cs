using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : Repository, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<Reservation> GetByIdAsync(long id)
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

        public async Task CreateAsync(Reservation reservation)
        {
            await _db.Reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _db.Entry(reservation).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
    }
}
