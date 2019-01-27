using ApplicationCore.Common;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.ReservationAggregate
{
    public class Reservation : Entity, IAggregateRoot
    {
        public long RoomItemId { get; private set; }
        public RoomItem RoomItem { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime CheckinDate { get; private set; }
        public DateTime CheckoutDate { get; private set; }
        public int NoOfNights => Math.Abs((CheckinDate - CheckinDate).Days - 1);
        public DateTime CreationDate { get; private set; } = DateTime.Now;
        public bool CheckedOut { get; private set; }
        public bool Canceled { get; private set; }

        private readonly List<ReservationFacility> _facilities = new List<ReservationFacility>();
        public IReadOnlyCollection<ReservationFacility> Facilities => _facilities.AsReadOnly();

        private Reservation()
        {
        }

        public Reservation(long roomItemId, Customer customer, DateTime checkinDate, DateTime checkoutDate)
        {
            Contract.Require(roomItemId > 0, "Room item id must be greater than 0");
            Contract.Require(customer != null, "Customer is required");
            Contract.Require(checkinDate < checkoutDate, "Check-in date must be before check-out date");

            int periodInDays = (checkoutDate - checkinDate).Days;
            Contract.Require(periodInDays <= 30, "Cannot reserve more than 30 days");

            RoomItemId = roomItemId;
            Customer = customer;
            CheckinDate = checkinDate;
            CheckoutDate = checkoutDate;
        }

        public void AddReservationFacility(ReservationFacility reservationFacility)
        {
            _facilities.Add(reservationFacility);
        }

        public void Cancel()
        {
            Canceled = true;
            CheckedOut = true;
        }
    }
}
