namespace ApplicationCore.Entities.HotelAggregate
{
    public class SingleRoom : Room
    {
        public SingleRoom(long hotelId, int floor, decimal pricePerNightInDollars)
            : base(hotelId, floor, "Single", pricePerNightInDollars)
        {
        }
    }
}
