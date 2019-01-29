namespace ApplicationCore.Entities.RoomAggregate
{
    public class SingleRoom : Room
    {
        public const string SingleRoomName = "Single";

        public SingleRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, SingleRoomName, pricePerNight)
        {
        }
    }
}
