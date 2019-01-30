using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<Hotel> GetFullByIdAsync(long id);
        Task<Result<List<RoomItem>>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate);
        Task<Result<IReadOnlyCollection<HotelFacility>>> GetFacilitiesByHotelIdAsync(long hotelId);
    }
}
