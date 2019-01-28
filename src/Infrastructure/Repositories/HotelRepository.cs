using ApplicationCore.Entities;
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
        private readonly IAppLogger<HotelRepository> _logger;
        private readonly IReservationRepository _reservationRepository;

        public HotelRepository(ApplicationDbContext db, IAppLogger<HotelRepository> logger, IReservationRepository reservationRepository) : base(db)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
        }

        public async Task<Result<List<RoomItem>>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            long[] reservedRoomItemIds = await _reservationRepository.GetIdsByHotelIdAndPeriodAsync(hotelId, checkinDate, checkoutDate);
            Hotel hotel = await _db.Hotels.Include(h => h.RoomItems).ThenInclude(r => r.Room).SingleOrDefaultAsync(h => h.Id == hotelId);
            if (hotel == null)
            {
                string message = $"Hote with id '{hotelId}' doesn't exists";
                _logger.LogInformation(message);
                return Result.Fail<List<RoomItem>>(message);
            }

            List<RoomItem> roomItems = hotel.RoomItems.Where(r => !reservedRoomItemIds.Contains(r.Id)).OrderBy(r => r.Number).ToList();
            return Result.Ok(roomItems);
        }

        public async Task<Result<IReadOnlyCollection<HotelFacility>>> GetFacilitiesByHotelIdAsync(long hotelId)
        {
            Hotel hotel = await _db.Hotels.FindAsync(hotelId);
            if (hotel == null)
            {
                string message = $"Hote with id '{hotelId}' doesn't exists";
                _logger.LogInformation(message);
                return Result.Fail<IReadOnlyCollection<HotelFacility>>(message);
            }

            return Result.Ok(hotel.Facilities);
        }
    }
}
