namespace ApplicationCore.Entities.HotelAggregate
{
    public class SingleRoom : Room
    {
        public SingleRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, "Single", pricePerNight)
        {
        }
    }
}
