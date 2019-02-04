namespace ApplicationCore.Entities.RoomAggregate
{
    public class StandardRoom : RoomType
    {
        public const string StandardRoomName = "Standard";

        public StandardRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, StandardRoomName, pricePerNight)
        {
        }
    }
}
