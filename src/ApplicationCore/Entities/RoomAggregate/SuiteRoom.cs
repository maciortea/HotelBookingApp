namespace ApplicationCore.Entities.RoomAggregate
{
    public class SuiteRoom : Room
    {
        public SuiteRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, "Suite", pricePerNight)
        {
        }
    }
}
