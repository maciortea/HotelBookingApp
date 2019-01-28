using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<List<RoomItem>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate);
        Task<IReadOnlyCollection<HotelFacility>> GetFacilitiesByHotelIdAsync(long hotelId);
    }
}
