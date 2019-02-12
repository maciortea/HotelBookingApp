using ApplicationCore.Entities.RoomTypeAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomTypeRepository : IRepository<RoomType>
    {
        Task<IReadOnlyCollection<RoomFacility>> GetFacilitiesByRoomIdAsync(long roomId);
        Task<IReadOnlyCollection<RoomFacility>> GetFacilitiesByIds(long roomId, long[] facilityIds);
    }
}
