namespace ApplicationCore.Entities.HotelAggregate
{
    public class SuiteRoom : Room
    {
        public SuiteRoom(long hotelId, int floor, decimal pricePerNightInDollars)
            : base(hotelId, floor, "Suite", pricePerNightInDollars)
        {
        }
    }
}
