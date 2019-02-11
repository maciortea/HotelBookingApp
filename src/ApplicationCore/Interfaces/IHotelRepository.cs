using ApplicationCore.Entities.HotelAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        Task<Dictionary<string, Tuple<int, decimal>>> GetRoomTypesToCountAndPrice(long hotelId);
    }
}
