using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class HotelFacilityRepository : Repository, IHotelFacilityRepository
    {
        public HotelFacilityRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<List<HotelFacility>> GetAllByHotelIdAsync(long hotelId)
        {
            return await _db.HotelFacilities.Where(f => f.HotelId == hotelId).ToListAsync();
        }
    }
}
