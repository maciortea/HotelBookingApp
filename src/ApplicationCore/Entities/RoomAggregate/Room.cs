using ApplicationCore.Common;
using ApplicationCore.SharedKernel;
using System.Collections.Generic;

namespace ApplicationCore.Entities.RoomAggregate
{
    public class Room : Entity
    {
        public long HotelId { get; private set; }
        public string Type { get; private set; }
        public Euros PricePerNight { get; private set; }

        private readonly List<RoomFacility> _facilities = new List<RoomFacility>();
        public IReadOnlyCollection<RoomFacility> Facilities => _facilities.AsReadOnly();

        private Room()
        {
        }

        public Room(long hotelId, string type, Euros pricePerNight)
        {
            Contract.Require(hotelId > 0, "Hotel id must be greater than 0");
            Contract.Require(!string.IsNullOrWhiteSpace(type), "Room type is required");

            HotelId = hotelId;
            Type = type;
            PricePerNight = pricePerNight;
        }

        public void AddFacility(RoomFacility facility)
        {
            _facilities.Add(facility);
        }
    }
}
