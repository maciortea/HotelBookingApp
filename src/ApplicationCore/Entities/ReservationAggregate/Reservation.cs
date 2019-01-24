using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities.ReservationAggregate
{
    public class Reservation : Entity, IAggregateRoot
    {
        public long RoomId { get; private set; }
        public Room Room { get; private set; } 
        public Customer Customer { get; private set; }
        public DateTime ReservationDate { get; private set; } = DateTime.Now;

        private Reservation()
        {
        }

        public Reservation(long roomId, Customer customer)
        {
            RoomId = roomId;
            Customer = customer;
        }
    }
}
