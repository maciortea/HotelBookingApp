using ApplicationCore.Entities.RoomTypeAggregate;
using ApplicationCore.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RoomTypeRepository : EfRepository<RoomType>, IRoomTypeRepository
    {
        public RoomTypeRepository(ApplicationDbContext db)
            : base(db)
        {
        }

        public async Task<IReadOnlyCollection<RoomFacility>> GetFacilitiesByRoomIdAsync(long roomId)
        {
            return await _db.RoomTypes.Where(r => r.Id == roomId).SelectMany(r => r.Facilities).ToListAsync();
        }

        public async Task<IReadOnlyCollection<RoomFacility>> GetFacilitiesByIds(long roomId, long[] facilityIds)
        {
            return await _db.RoomTypes
                .Where(r => r.Id == roomId)
                .SelectMany(r => r.Facilities.Where(f => facilityIds.Contains(f.Id)))
                .ToListAsync();
        }
    }
}
