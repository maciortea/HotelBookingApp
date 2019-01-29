using System.Collections.Generic;
using System.ComponentModel;

namespace Web.Models.Reservation
{
    public class ReservationCheckoutViewModel
    {
        public long Id { get; set; }

        [DisplayName("Room type")]
        public string RoomType { get; set; }

        [DisplayName("Customer")]
        public string CustomerFullName { get; set; }

        [DisplayName("No of nights")]
        public int NoOfNights { get; set; }

        public List<FacilityViewModel> HotelFacilities { get; set; }
        public List<FacilityViewModel> RoomFacilities { get; set; }

        public ReservationCheckoutViewModel()
        {
            HotelFacilities = new List<FacilityViewModel>();
            RoomFacilities = new List<FacilityViewModel>();
        }
    }
}
