using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using System;

namespace ApplicationCore.Entities.ReservationAggregate
{
    public class Reservation : Entity, IAggregateRoot
    {
        public long RoomId { get; set; }
        public Room Room { get; set; } 
        public Customer Customer { get; set; }
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
