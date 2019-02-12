using ApplicationCore.Entities;
using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.Specifications
{
    public class ReservationWithFullMembersSpecificationTest
    {
        [Fact]
        public void GetsHotelWithSpecifiedId()
        {
            var specification = new ReservationWithFullMembersSpecification(2);

            Reservation reservation = GetTestReservations().AsQueryable().SingleOrDefault(specification.Criteria);

            Assert.NotNull(reservation);
            Assert.Equal(2, reservation.Id);
        }

        private List<Reservation> GetTestReservations()
        {
            var customer = new Customer("Michael", "Smith", "00444567123");

            return new List<Reservation>
            {
                new Reservation(1, customer, DateTime.Now, DateTime.Now.AddDays(1)) { Id = 1 },
                new Reservation(2, customer, DateTime.Now, DateTime.Now.AddDays(1)) { Id = 2 },
                new Reservation(3, customer, DateTime.Now, DateTime.Now.AddDays(1)) { Id = 3 },
            };
        }
    }
}
