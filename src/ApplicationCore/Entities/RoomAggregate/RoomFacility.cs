using ApplicationCore.Common;

namespace ApplicationCore.Entities.RoomAggregate
{
    public class RoomFacility : Facility
    {
        public long RoomTypeId { get; private set; }

        private RoomFacility()
        {
        }

        public RoomFacility(string name, Euros unitPrice, bool freeOfCharge, long roomTypeId)
            : base(name, unitPrice, freeOfCharge)
        {
            Contract.Require(roomTypeId > 0, "Room type id must be greater than 0");

            RoomTypeId = roomTypeId;
        }
    }
}
