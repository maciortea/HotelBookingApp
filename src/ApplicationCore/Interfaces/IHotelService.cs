using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHotelService
    {
        Task<Result<Hotel>> GetFullByIdAsync(long id);
        Task<Result<List<Room>>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime fromDate, DateTime toDate);
        Task<Result<IReadOnlyCollection<HotelFacility>>> GetFacilitiesByHotelIdAsync(long hotelId);
        Task<Result<Dictionary<string, Tuple<int, decimal>>>> GetRoomTypesToCountAndPrice(long hotelId);
    }
}
