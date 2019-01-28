using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class HotelRepository : EfRepository<Hotel>, IHotelRepository
    {
        private readonly IReservationRepository _reservationRepository;

        public HotelRepository(ApplicationDbContext db, IReservationRepository reservationRepository) : base(db)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<List<RoomItem>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            long[] reservedRoomItemIds = await _reservationRepository.GetIdsByHotelIdAndPeriodAsync(hotelId, checkinDate, checkoutDate);
            Hotel hotel = await _db.Hotels.Include(h => h.RoomItems).ThenInclude(r => r.Room).SingleOrDefaultAsync(h => h.Id == hotelId);
            return hotel.RoomItems.Where(r => !reservedRoomItemIds.Contains(r.Id)).OrderBy(r => r.Number).ToList();
        }

        public async Task<IReadOnlyCollection<HotelFacility>> GetFacilitiesByHotelIdAsync(long hotelId)
        {
            Hotel hotel = await _db.Hotels.FindAsync(hotelId);
            return hotel.Facilities;
        }
    }
}
