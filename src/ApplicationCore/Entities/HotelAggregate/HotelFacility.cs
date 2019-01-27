namespace ApplicationCore.Entities.HotelAggregate
{
    public class HotelFacility : Facility
    {
        public long HotelId { get; private set; }

        private HotelFacility()
        {
        }

        public HotelFacility(string name, Euros unitPrice, bool freeOfCharge, long hotelId)
            : base(name, unitPrice, freeOfCharge)
        {
            HotelId = hotelId;
        }
    }
}
