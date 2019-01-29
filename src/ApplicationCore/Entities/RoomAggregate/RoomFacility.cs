using ApplicationCore.Common;

namespace ApplicationCore.Entities.RoomAggregate
{
    public class RoomFacility : Facility
    {
        public long RoomId { get; private set; }

        private RoomFacility()
        {
        }

        public RoomFacility(string name, Euros unitPrice, bool freeOfCharge, long roomId)
            : base(name, unitPrice, freeOfCharge)
        {
            Contract.Require(roomId > 0, "Room id must be greater than 0");

            RoomId = roomId;
        }
    }
}
