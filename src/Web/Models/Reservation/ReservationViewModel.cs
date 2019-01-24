using System;
using System.ComponentModel;

namespace Web.Models.Reservation
{
    public class ReservationViewModel
    {
        [DisplayName("Room type")]
        public string RoomType { get; set; }

        [DisplayName("Reservation date")]
        public DateTime ReservationDate { get; set; }
    }
}
