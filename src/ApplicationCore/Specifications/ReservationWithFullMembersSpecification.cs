using ApplicationCore.Entities.ReservationAggregate;

namespace ApplicationCore.Specifications
{
    public class ReservationWithFullMembersSpecification : Specification<Reservation>
    {
        public ReservationWithFullMembersSpecification(long id)
            : base(r => r.Id == id)
        {
            AddInclude("Facilities.HotelFacility");
            AddInclude("Room.RoomType");
        }
    }
}
