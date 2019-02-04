namespace ApplicationCore.Entities.RoomTypeAggregate
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
