using ApplicationCore.Entities.HotelAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomRepository
    {
        Task<List<RoomItem>> GetAvailableByHotelIdAndPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate);
    }
}
