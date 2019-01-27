namespace ApplicationCore.Entities.HotelAggregate
{
    public class StandardRoom : Room
    {
        public StandardRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, "Standard", pricePerNight)
        {
        }
    }
}
