using ApplicationCore.Entities.HotelAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IHotelFacilityRepository
    {
        Task<List<HotelFacility>> GetAllByHotelIdAsync(long hotelId);
    }
}
