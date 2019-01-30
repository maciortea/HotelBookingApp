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

        public HotelViewModel()
        {
            Facilities = new List<string>();
        }
    }
}
