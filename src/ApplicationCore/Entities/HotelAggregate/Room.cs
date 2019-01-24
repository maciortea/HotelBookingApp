using ApplicationCore.Common;

namespace ApplicationCore.Entities.HotelAggregate
{
    public class Room : Entity
    {
        public long HotelId { get; private set; }
        public int Floor { get; private set; }
        public string Type { get; private set; }
        public decimal PricePerNightInDollars { get; private set; }

        public Room(long hotelId, int floor, string type, decimal pricePerNightInDollars)
        {
            Contract.Require(floor >= 0, "Cannot have negative floor");
            Contract.Require(floor <= 10, "Maximum 10 floors allowed"); // put this in config

            HotelId = hotelId;
            Floor = floor;
            Type = type;
            PricePerNightInDollars = pricePerNightInDollars;
        }
    }
}
