using System.ComponentModel;

namespace Web.Models.Reservation
{
    public class ReservationViewModel
    {
        [DisplayName("Room type")]
        public string RoomType { get; set; }

        [DisplayName("Customer")]
        public string CustomerFullName { get; set; }

        public ReservationPeriodViewModel ReservationPeriod { get; set; }
    }
}
