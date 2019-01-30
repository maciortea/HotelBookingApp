using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Web.Models.Hotel
{
    public class HotelViewModel
    {
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Address")]
        public string FullAddress { get; set; }

        [DisplayName("Facilities")]
        public List<string> Facilities { get; set; }

        [DisplayName("Rooms")]
        public Dictionary<string, Tuple<int, decimal>> RoomTypesToCountAndPrice { get; set; }

        public HotelViewModel()
        {
            Facilities = new List<string>();
            RoomTypesToCountAndPrice = new Dictionary<string, Tuple<int, decimal>>();
        }
    }
}
