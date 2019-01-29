using ApplicationCore.Common;

namespace ApplicationCore.Entities
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
            Contract.Require(hotelId > 0, "Hotel id must be greater than 0");

            HotelId = hotelId;
        }
    }
}
