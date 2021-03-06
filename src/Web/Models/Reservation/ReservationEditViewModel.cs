﻿using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Reservation
{
    public class ReservationEditViewModel
    {
        [DisplayName("Available room types")]
        [Required]
        public long RoomId { get; set; }

        [DisplayName("First name")]
        [Required]
        public string CustomerFirstName { get; set; }

        [DisplayName("Last name")]
        [Required]
        public string CustomerLastName { get; set; }

        [DisplayName("Phone number")]
        [Required]
        public string CustomerPhone { get; set; }

        [DisplayName("Hotel facilities")]
        public List<long> HotelFacilityIds { get; set; }

        public ReservationPeriodViewModel ReservationPeriod { get; set; }

        public List<RoomViewModel> AvailableRoomTypes { get; set; }

        public List<FacilityViewModel> HotelFacilities { get; set; }

        public ReservationEditViewModel()
        {
            HotelFacilityIds = new List<long>();
            AvailableRoomTypes = new List<RoomViewModel>();
            HotelFacilities = new List<FacilityViewModel>();
        }
    }
}
