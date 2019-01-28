namespace ApplicationCore.Entities.RoomAggregate
{
    public class StandardRoom : Room
    {
        public StandardRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, "Standard", pricePerNight)
        {
        }
    }
}
