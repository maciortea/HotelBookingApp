using ApplicationCore.Entities.RoomAggregate;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<Result<IReadOnlyCollection<RoomFacility>>> GetFacilitiesByRoomIdAsync(long roomId);
        Task<Result<IReadOnlyCollection<RoomFacility>>> GetFacilitiesByIds(long roomId, long[] facilityIds);
    }
}
