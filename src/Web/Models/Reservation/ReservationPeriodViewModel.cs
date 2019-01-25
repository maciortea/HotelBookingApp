using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Reservation
{
    public class ReservationPeriodViewModel
    {
        [DisplayName("Check-in date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime CheckinDate { get; set; }

        [DisplayName("Check-out date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime CheckoutDate { get; set; }

        public ReservationPeriodViewModel()
        {
        }

        public ReservationPeriodViewModel(DateTime checkinDate, DateTime checkoutDate)
        {
            // validate that checkin date is before checkout date
            // constrain this rule in properties
            CheckinDate = checkinDate;
            CheckoutDate = checkoutDate;
        }
    }
}
