using ApplicationCore.Entities.HotelAggregate;

namespace ApplicationCore.Specifications
{
    public class HotelWithFullMembersSpecification : Specification<Hotel>
    {
        public HotelWithFullMembersSpecification(long id)
            : base(h => h.Id == id)
        {
            AddInclude(h => h.Facilities);
            AddInclude("Rooms.RoomType");
        }
    }
}
