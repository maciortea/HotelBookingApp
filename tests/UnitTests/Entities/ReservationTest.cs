using ApplicationCore.Common;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ReservationAggregate;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class ReservationTest
    {
        [Fact]
        public void Constructor_WithInvalidArguments_ShouldThrowContractException()
        {
            Customer customer = CreateCustomer();
            Func<Reservation> createReservation1 = () => new Reservation(-1, customer, DateTime.Today, DateTime.Today.AddDays(1));
            Func <Reservation> createReservation2 = () => new Reservation(0, customer, DateTime.Today, DateTime.Today.AddDays(1));
            Assert.Throws<ContractException>(createReservation1);
            Assert.Throws<ContractException>(createReservation1);
        }

        [Fact]
        public void Constructor_WithCheckingDateAfterCheckoutDate_ShouldThrowContractException()
        {
            Customer customer = CreateCustomer();
            Func<Reservation> createReservation = () => new Reservation(1, customer, DateTime.Today, DateTime.Today.AddDays(-1));
            Assert.Throws<ContractException>(createReservation);
        }

        [Fact]
        public void Constructor_WithCheckingDateSameAsCheckoutDate_ShouldThrowContractException()
        {
            Customer customer = CreateCustomer();
            Func<Reservation> createReservation = () => new Reservation(1, customer, DateTime.Today, DateTime.Today);
            Assert.Throws<ContractException>(createReservation);
        }

        [Fact]
        public void Constructor_WithPeriodHigherThan30Days_ShouldThrowContractException()
        {
            Customer customer = CreateCustomer();
            Func<Reservation> createReservation = () => new Reservation(1, customer, DateTime.Today, DateTime.Today);
            Assert.Throws<ContractException>(createReservation);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            Customer customer = CreateCustomer();
            var checkinDate = DateTime.Today;
            var checkoutDate = checkinDate.AddDays(1);
            var reservation = new Reservation(1, customer, checkinDate, checkoutDate);
            Assert.Equal(1, reservation.RoomItemId);
            Assert.Same(customer, reservation.Customer);
            Assert.Equal(checkinDate, reservation.CheckinDate);
            Assert.Equal(checkoutDate, reservation.CheckoutDate);
        }

        [Fact]
        public void AddReservationFacility_ShouldAddToList()
        {
            Reservation reservation = CreateValidReservation();
            reservation.AddReservationFacility(new ReservationFacility(1, 1));
            Assert.Equal(1, reservation.Facilities.Count);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateAndCheckoutDateInFutureForAPeriodOfFourDays_ShouldReturnThree()
        {
            var currentDate = new DateTime(2019, 1, 1);
            var checkinDate = new DateTime(2019, 1, 2);
            var checkoutDate = new DateTime(2019, 1, 5);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(3, noOfNights);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateSameAsCurrentDateAndCheckoutDateInFutureForAPeriodOfFourDays_ShouldReturnOne()
        {
            var currentDate = new DateTime(2019, 1, 1);
            var checkinDate = new DateTime(2019, 1, 1);
            var checkoutDate = new DateTime(2019, 1, 4);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(1, noOfNights);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateInPastAndCheckoutDateSameAsCurrentDateForAPeriodOfFourDays_ShouldReturnThree()
        {
            var currentDate = new DateTime(2019, 1, 4);
            var checkinDate = new DateTime(2019, 1, 1);
            var checkoutDate = new DateTime(2019, 1, 4);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(3, noOfNights);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateInPastAndCheckoutDateInPastForAPeriodOfFourDays_ShouldReturnThree()
        {
            var currentDate = new DateTime(2019, 1, 5);
            var checkinDate = new DateTime(2019, 1, 1);
            var checkoutDate = new DateTime(2019, 1, 4);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(3, noOfNights);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateAndCheckoutDateInFutureForAPeriodOfTwoDays_ShouldReturnOne()
        {
            var currentDate = new DateTime(2019, 1, 1);
            var checkinDate = new DateTime(2019, 1, 2);
            var checkoutDate = new DateTime(2019, 1, 3);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(1, noOfNights);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateSameAsCurrentDateAndCheckoutDateInFutureForAPeriodOfTwoDays_ShouldReturnOne()
        {
            var currentDate = new DateTime(2019, 1, 1);
            var checkinDate = new DateTime(2019, 1, 1);
            var checkoutDate = new DateTime(2019, 1, 2);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(1, noOfNights);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateInPastAndCheckoutDateSameAsCurrentDateForAPeriodOfTwoDays_ShouldReturnOne()
        {
            var currentDate = new DateTime(2019, 1, 4);
            var checkinDate = new DateTime(2019, 1, 3);
            var checkoutDate = new DateTime(2019, 1, 4);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(1, noOfNights);
        }

        [Fact]
        public void CalculateCheckoutNoOfNights_WithCheckinDateInPastAndCheckoutDateInPastForAPeriodOfTwoDays_ShouldReturnOne()
        {
            var currentDate = new DateTime(2019, 1, 5);
            var checkinDate = new DateTime(2019, 1, 1);
            var checkoutDate = new DateTime(2019, 1, 2);
            Reservation reservation = CreateValidReservation(checkinDate, checkoutDate);
            int noOfNights = reservation.CalculateCheckoutNoOfNights(currentDate);
            Assert.Equal(1, noOfNights);
        }

        [Fact]
        public void Checkout_ShouldSetCanceledToFalseAndCheckedOutToTrue()
        {
            Reservation reservation = CreateValidReservation();
            reservation.Checkout();
            Assert.False(reservation.Canceled);
            Assert.True(reservation.CheckedOut);
        }

        [Fact]
        public void Cancel_ShouldSetCanceledToTrueAndCheckedOutToTrue()
        {
            Reservation reservation = CreateValidReservation();
            reservation.Cancel();
            Assert.True(reservation.Canceled);
            Assert.True(reservation.CheckedOut);
        }

        private Customer CreateCustomer()
        {
            return new Customer("first", "last", "123");
        }

        private Reservation CreateValidReservation()
        {
            Customer customer = CreateCustomer();
            return new Reservation(1, customer, DateTime.Today, DateTime.Today.AddDays(1));
        }

        private Reservation CreateValidReservation(DateTime checkinDate, DateTime checkoutDate)
        {
            Customer customer = CreateCustomer();
            return new Reservation(1, customer, checkinDate, checkoutDate);
        }
    }
}
