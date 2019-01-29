using ApplicationCore.Common;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.SharedKernel;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.ReservationAggregate
{
    public class Reservation : AggregateRoot
    {
        public const int MaximumAllowedDays = 30;
        public const int MinimumNoOfNights = 1;

        public long RoomItemId { get; private set; }
        public RoomItem RoomItem { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime CheckinDate { get; private set; }
        public DateTime CheckoutDate { get; private set; }
        public DateTime CreationDate { get; private set; } = DateTime.Now;
        public DateTime? ActualCheckoutDate { get; private set; }
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
            Contract.Require(periodInDays <= MaximumAllowedDays, $"Cannot reserve more than {MaximumAllowedDays} days");

            RoomItemId = roomItemId;
            Customer = customer;
            CheckinDate = checkinDate;
            CheckoutDate = checkoutDate;
        }

        public void AddReservationFacility(ReservationFacility reservationFacility)
        {
            _facilities.Add(reservationFacility);
        }

        public int CalculateCheckoutNoOfNights(DateTime currentDate)
        {
            if (CheckinDate <= currentDate && CheckoutDate > currentDate)
            {
                if (currentDate == CheckinDate)
                {
                    // Checkin date is same as current date and checkout date is in future
                    // This means that customer will be charged for one night stay
                    return MinimumNoOfNights;
                }

                // Checkin date is in past and checkout date is in future
                return Math.Abs((currentDate - CheckinDate).Days);
            }

            return Math.Abs((CheckoutDate - CheckinDate).Days);
        }

        public void Checkout()
        {
            Close(false, true);
        }

        public void Cancel()
        {
            Close(true, true);
        }

        private void Close(bool canceled, bool checkedOut)
        {
            Canceled = canceled;
            CheckedOut = checkedOut;
            ActualCheckoutDate = DateTime.Today;
        }
    }
}
