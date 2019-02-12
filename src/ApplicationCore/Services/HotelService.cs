using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class HotelService : IHotelService
    {
        private readonly IAppLogger<HotelService> _logger;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRepository<Reservation> _reservationRepository;

        public HotelService(
            IAppLogger<HotelService> logger,
            IHotelRepository hotelRepository,
            IRepository<Reservation> reservationRepository)
        {
            _logger = logger;
            _hotelRepository = hotelRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<Result<Hotel>> GetFullByIdAsync(long id)
        {
            try
            {
                var specification = new HotelWithFullMembersSpecification(id);
                Hotel hotel = await _hotelRepository.SingleOrDefaultAsync(specification);
                if (hotel == null)
                {
                    string message = $"Hote with id '{id}' doesn't exists";
                    _logger.LogInformation(message);
                    return Result.Fail<Hotel>(message);
                }

                return Result.Ok(hotel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<Hotel>(ex.Message);
            }
        }

        public async Task<Result<List<Room>>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                long[] reservedRoomItemIds = await GetBookedRoomItemIdsByHotelIdAndPeriodAsync(hotelId, fromDate, toDate);

                var hotelResult = await GetFullByIdAsync(hotelId);
                if (hotelResult.IsFailure)
                {
                    return Result.Fail<List<Room>>(hotelResult.Error);
                }

                List<Room> rooms = hotelResult.Value.Rooms.Where(r => !reservedRoomItemIds.Contains(r.Id)).OrderBy(r => r.Number).ToList();
                return Result.Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<List<Room>>(ex.Message);
            }
        }

        public async Task<Result<IReadOnlyCollection<HotelFacility>>> GetFacilitiesByHotelIdAsync(long hotelId)
        {
            try
            {
                var hotelResult = await GetFullByIdAsync(hotelId);
                if (hotelResult.IsFailure)
                {
                    return Result.Fail<IReadOnlyCollection<HotelFacility>> (hotelResult.Error);
                }

                return Result.Ok(hotelResult.Value.Facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<IReadOnlyCollection<HotelFacility>>(ex.Message);
            }
        }

        public async Task<Result<Dictionary<string, Tuple<int, decimal>>>> GetRoomTypesToCountAndPrice(long hotelId)
        {
            try
            {
                var roomTypesToCountAndPrice = await _hotelRepository.GetRoomTypesToCountAndPrice(hotelId);
                return Result.Ok(roomTypesToCountAndPrice);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<Dictionary<string, Tuple<int, decimal>>>(ex.Message);
            }
        }

        private async Task<long[]> GetBookedRoomItemIdsByHotelIdAndPeriodAsync(long hotelId, DateTime fromDate, DateTime toDate)
        {
            var specification = new BookedRoomItemIdsByHotelIdAndPeriodSpecification(hotelId, fromDate, toDate);
            var reservations = await _reservationRepository.ListAsync(specification);
            return reservations.Select(r => r.RoomId).ToArray();
        }
    }
}
