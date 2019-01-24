using ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class Hotel : Entity, IAggregateRoot
    {
        public string Name { get; }
        public Address Address { get; }

        private readonly List<Room> _rooms;
        public IReadOnlyCollection<Room> Rooms => _rooms.AsReadOnly();

        private Hotel()
        {
        }

        public Hotel(string name, Address address)
        {
            Name = name;
            Address = address;
            _rooms = new List<Room>();
        }

        public void AddRoom(Room room)
        {
            _rooms.Add(room);
        }
    }
}
