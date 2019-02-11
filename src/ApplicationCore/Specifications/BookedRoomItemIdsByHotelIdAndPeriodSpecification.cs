using ApplicationCore.Entities.ReservationAggregate;
using System;

namespace ApplicationCore.Specifications
{
    public class BookedRoomItemIdsByHotelIdAndPeriodSpecification : Specification<Reservation>
    {
        public BookedRoomItemIdsByHotelIdAndPeriodSpecification(long hotelId, DateTime fromDate, DateTime toDate)
            : base(r =>
                r.Room.RoomType.HotelId == hotelId &&
                (r.CheckinDate >= fromDate && r.CheckinDate < toDate ||
                 r.CheckinDate < fromDate && r.CheckoutDate > fromDate) &&
                !r.CheckedOut &&
                !r.Canceled)
        {
        }
    }
}
