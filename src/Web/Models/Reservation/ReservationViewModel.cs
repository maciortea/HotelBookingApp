using System;
using System.ComponentModel;

namespace Web.Models.Reservation
{
    public class ReservationViewModel
    {
        private DateTime _currentDate;

        public long Id { get; set; }

        [DisplayName("Room type")]
        public string RoomType { get; set; }

        [DisplayName("Customer")]
        public string CustomerFullName { get; set; }

        public ReservationPeriodViewModel ReservationPeriod { get; set; }

        [DisplayName("Created on")]
        public DateTime CreationDate { get; set; }

        public bool CanCheckout => ReservationPeriod.CheckoutDate <= _currentDate;

        public ReservationViewModel(DateTime currentDate)
        {
            _currentDate = currentDate;
        }
    }
}
