using ApplicationCore.SharedKernel;
using System.Collections.Generic;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class Hotel : AggregateRoot
    {
        public string Name { get; private set; }
        public Address Address { get; private set; }

        private readonly List<Room> _rooms = new List<Room>();
        public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();

        private readonly List<HotelFacility> _facilities = new List<HotelFacility>();
        public IReadOnlyCollection<HotelFacility> Facilities => _facilities.AsReadOnly();

        private Hotel()
        {
        }

        public Hotel(string name, Address address)
        {
            Name = name;
            Address = address;
        }

        public void AddRoom(Room room)
        {
            _rooms.Add(room);
        }

        public void AddFacility(HotelFacility facility)
        {
            _facilities.Add(facility);
        }
    }
}
