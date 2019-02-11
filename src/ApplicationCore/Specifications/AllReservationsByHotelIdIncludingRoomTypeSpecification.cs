using ApplicationCore.Entities.ReservationAggregate;

namespace ApplicationCore.Specifications
{
    public class AllReservationsByHotelIdIncludingRoomTypeSpecification : Specification<Reservation>
    {
        public AllReservationsByHotelIdIncludingRoomTypeSpecification(long hotelId)
            : base(r => r.Room.RoomType.HotelId == hotelId && !r.CheckedOut && !r.Canceled)
        {
            AddInclude("Room.RoomType");
        }
    }
}
