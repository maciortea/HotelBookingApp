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

        public HotelRepository(ApplicationDbContext db, IAppLogger<HotelRepository> logger) : base(db)
        {
            _logger = logger;
        }

        public async Task<Result<List<RoomItem>>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate)
        {
            try
            {
                long[] reservedRoomItemIds = await GetBookedRoomItemIdsByHotelIdAndPeriodAsync(hotelId, checkinDate, checkoutDate);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<List<RoomItem>>(ex.Message);
            }
        }

        public async Task<Result<IReadOnlyCollection<HotelFacility>>> GetFacilitiesByHotelIdAsync(long hotelId)
        {
            try
            {
                Hotel hotel = await _db.Hotels.Include(h => h.Facilities).SingleOrDefaultAsync(h => h.Id == hotelId);
                if (hotel == null)
                {
                    string message = $"Hote with id '{hotelId}' doesn't exists";
                    _logger.LogInformation(message);
                    return Result.Fail<IReadOnlyCollection<HotelFacility>>(message);
                }

                return Result.Ok(hotel.Facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<IReadOnlyCollection<HotelFacility>>(ex.Message);
            }
        }

        private async Task<long[]> GetBookedRoomItemIdsByHotelIdAndPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate)
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
