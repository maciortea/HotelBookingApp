using ApplicationCore.Entities.RoomAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoomRepository : EfRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<IReadOnlyCollection<RoomFacility>> GetAllByRoomIdAsync(long roomId)
        {
            return await _db.Rooms.Where(r => r.Id == roomId).SelectMany(r => r.Facilities).ToListAsync();
        }

        public async Task<IReadOnlyCollection<RoomFacility>> GetFacilitiesByIds(long roomId, long[] facilityIds)
        {
            return await _db.Rooms
                .Where(r => r.Id == roomId)
                .SelectMany(r => r.Facilities.Where(f => facilityIds.Contains(f.Id)))
                .ToListAsync();
        }
    }
}
