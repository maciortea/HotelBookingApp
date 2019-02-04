using ApplicationCore.Common;
using ApplicationCore.Entities.RoomTypeAggregate;
using ApplicationCore.SharedKernel;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class Room : Entity
    {
        public long RoomTypeId { get; private set; }
        public RoomType RoomType { get; private set; }
        public int Floor { get; private set; }
        public int Number { get; private set; }

        public Room(long roomTypeId, int floor, int number)
        {
            Contract.Require(roomTypeId > 0, "Room type id must be greater than 0");
            Contract.Require(floor >= 0, "Cannot have negative floor");
            Contract.Require(floor <= 10, "Maximum 10 floors allowed");
            Contract.Require(number > 0, "Room number must be greater than 0");

            RoomTypeId = roomTypeId;
            Floor = floor;
            Number = number;
        }
    }
}
