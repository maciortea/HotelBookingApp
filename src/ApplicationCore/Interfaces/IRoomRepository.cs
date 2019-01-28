using ApplicationCore.Entities.RoomAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRoomRepository
    {
        Task<IReadOnlyCollection<RoomFacility>> GetAllByRoomIdAsync(long roomId);
    }
}
