using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Reservation
{
    public class ReservationPeriodViewModel
    {
        [DisplayName("Check-in date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Required]
        public DateTime CheckinDate { get; set; }

        [DisplayName("Check-out date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [Required]
        public DateTime CheckoutDate { get; set; }

        public ReservationPeriodViewModel()
        {
        }

        public ReservationPeriodViewModel(DateTime checkinDate, DateTime checkoutDate)
        {
            CheckinDate = checkinDate;
            CheckoutDate = checkoutDate;
        }
    }
}
