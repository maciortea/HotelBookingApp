using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.SharedKernel;

namespace ApplicationCore.Entities.ReservationAggregate
{
    public class ReservationFacility : Entity
    {
        public long ReservationId { get; private set; }
        public Reservation Reservation { get; private set; }
        public long HotelFacilityId { get; private set; }
        public HotelFacility HotelFacility { get; private set; }

        public ReservationFacility(long reservationId, long hotelFacilityId)
        {
            ReservationId = reservationId;
            HotelFacilityId = hotelFacilityId;
        }
    }
}
