namespace ApplicationCore.Entities.RoomAggregate
{
    public class SingleRoom : Room
    {
        public SingleRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, "Single", pricePerNight)
        {
        }
    }
}
