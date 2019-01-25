using ApplicationCore.Common;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities.ReservationAggregate
{
    public class Reservation : Entity, IAggregateRoot
    {
        public long RoomId { get; private set; }
        public Room Room { get; private set; } 
        public Customer Customer { get; private set; }
        public DateTime CheckinDate { get; private set; }
        public DateTime CheckoutDate { get; private set; }
        public DateTime CreationDate { get; private set; } = DateTime.Now;

        private Reservation()
        {
        }

        public Reservation(long roomId, Customer customer, DateTime checkinDate, DateTime checkoutDate)
        {
            Contract.Require(roomId > 0, "Room id must be greater than 0");
            Contract.Require(customer != null, "Customer is required");
            Contract.Require(checkinDate < checkoutDate, "Check-in date must be before check-out date");

            int periodInDays = (checkoutDate - checkinDate).Days;
            Contract.Require(periodInDays <= 30, "Cannot reserve more than 30 days");

            RoomId = roomId;
            Customer = customer;
            CheckinDate = checkinDate;
            CheckoutDate = checkoutDate;
        }
    }
}
