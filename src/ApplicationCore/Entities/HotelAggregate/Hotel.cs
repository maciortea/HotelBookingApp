using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class Hotel : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Address Address { get; private set; }

        private readonly List<Room> _rooms = new List<Room>();
        public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();

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
    }
}
