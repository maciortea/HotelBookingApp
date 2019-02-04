using ApplicationCore.Entities.RoomTypeAggregate;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        Task<Result<IReadOnlyCollection<RoomFacility>>> GetFacilitiesByRoomIdAsync(long roomId);
        Task<Result<IReadOnlyCollection<RoomFacility>>> GetFacilitiesByIds(long roomId, long[] facilityIds);
    }
}
