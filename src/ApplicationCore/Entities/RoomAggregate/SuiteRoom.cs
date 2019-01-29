namespace ApplicationCore.Entities.RoomAggregate
{
    public class SuiteRoom : Room
    {
        public const string SuiteRoomName = "Suite";

        public SuiteRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, SuiteRoomName, pricePerNight)
        {
        }
    }
}
