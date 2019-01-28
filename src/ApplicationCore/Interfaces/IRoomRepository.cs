using ApplicationCore.Entities.RoomAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<IReadOnlyCollection<RoomFacility>> GetAllByRoomIdAsync(long roomId);
        Task<IReadOnlyCollection<RoomFacility>> GetFacilitiesByIds(long roomId, long[] facilityIds);
    }
}
