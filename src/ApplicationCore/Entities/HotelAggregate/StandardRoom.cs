namespace ApplicationCore.Entities.HotelAggregate
{
    public class StandardRoom : Room
    {
        public StandardRoom(long hotelId, int floor, decimal pricePerNightInDollars)
            : base(hotelId, floor, "Standard", pricePerNightInDollars)
        {
        }
    }
}
