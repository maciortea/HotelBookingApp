using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime CreationDate { get; set; }

        public int Floor { get; set; }

        public CheckoutStatus CheckoutStatus
        {
            get
            {
                if (ReservationPeriod.CheckinDate > _currentDate && ReservationPeriod.CheckoutDate > _currentDate)
                {
                    return CheckoutStatus.Cancel;
                }
                else if (ReservationPeriod.CheckoutDate > _currentDate)
                {
                    return CheckoutStatus.Early;
                }
                else
                {
                    return CheckoutStatus.Complete;
                }
            }
        }

        public ReservationViewModel(DateTime currentDate)
        {
            _currentDate = currentDate;
        }
    }
}
