using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class Hotel : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Address Address { get; private set; }

        private readonly List<RoomItem> _roomItems = new List<RoomItem>();
        public IReadOnlyCollection<RoomItem> RoomItems => _roomItems.AsReadOnly();

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

        public void AddRoomItem(RoomItem roomItem)
        {
            _roomItems.Add(roomItem);
        }

        public void AddFacility(HotelFacility facility)
        {
            _facilities.Add(facility);
        }
    }
}
