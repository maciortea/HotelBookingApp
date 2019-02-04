namespace ApplicationCore.Entities.RoomAggregate
{
    public class SingleRoom : RoomType
    {
        public const string SingleRoomName = "Single";

        public SingleRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, SingleRoomName, pricePerNight)
        {
        }
    }
}
