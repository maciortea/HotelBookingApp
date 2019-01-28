using ApplicationCore.Common;
using ApplicationCore.SharedKernel;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class RoomItem : Entity
    {
        public long RoomId { get; private set; }
        public Room Room { get; private set; }
        public int Floor { get; private set; }
        public int Number { get; private set; }

        public RoomItem(long roomId, int floor, int number)
        {
            Contract.Require(roomId > 0, "Room id must be greater than 0");
            Contract.Require(floor >= 0, "Cannot have negative floor");
            Contract.Require(floor <= 10, "Maximum 10 floors allowed");
            Contract.Require(number > 0, "Room number must be greater than 0");

            RoomId = roomId;
            Floor = floor;
            Number = number;
        }
    }
}
