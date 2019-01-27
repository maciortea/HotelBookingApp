namespace ApplicationCore.Entities.HotelAggregate
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
            RoomId = roomId;
        }
    }
}
