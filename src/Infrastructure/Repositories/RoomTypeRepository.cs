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
        private readonly IAppLogger<RoomTypeRepository> _logger;

        public RoomTypeRepository(ApplicationDbContext db, IAppLogger<RoomTypeRepository> logger) : base(db)
        {
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<RoomFacility>>> GetFacilitiesByRoomIdAsync(long roomId)
        {
            try
            {
                IReadOnlyCollection<RoomFacility> facilities = await _db.RoomTypes.Where(r => r.Id == roomId).SelectMany(r => r.Facilities).ToListAsync();
                return Result.Ok(facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<IReadOnlyCollection<RoomFacility>>(ex.Message);
            }
        }

        public async Task<Result<IReadOnlyCollection<RoomFacility>>> GetFacilitiesByIds(long roomId, long[] facilityIds)
        {
            try
            {
                IReadOnlyCollection<RoomFacility> facilities = await _db.RoomTypes
                    .Where(r => r.Id == roomId)
                    .SelectMany(r => r.Facilities.Where(f => facilityIds.Contains(f.Id)))
                    .ToListAsync();

                return Result.Ok(facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<IReadOnlyCollection<RoomFacility>>(ex.Message);
            }
        }
    }
}
