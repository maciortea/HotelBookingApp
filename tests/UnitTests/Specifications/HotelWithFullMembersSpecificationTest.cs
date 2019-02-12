using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Specifications;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Specifications
{
    public class HotelWithFullMembersSpecificationTest
    {
        [Fact]
        public void GetsHotelWithSpecifiedId()
        {
            var specification = new HotelWithFullMembersSpecification(2);

            Hotel hotel = GetTestHotels().AsQueryable().SingleOrDefault(specification.Criteria);

            Assert.NotNull(hotel);
            Assert.Equal(2, hotel.Id);
        }

        private List<Hotel> GetTestHotels()
        {
            var address = new Address("Great George St", "London", "UK", "E1 0AE");

            return new List<Hotel>
            {
                new Hotel("hotel1", address) { Id = 1 },
                new Hotel("hotel2", address) { Id = 2 },
                new Hotel("hotel3", address) { Id = 3 }
            };
        }
    }
}
