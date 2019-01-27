namespace ApplicationCore.Entities.HotelAggregate
{
    public class FacilityFactory
    {
        public static HotelFacility CreateFreeHotelFacility(string name, long hotelId)
        {
            return new HotelFacility(name, Euros.Of(0), true, hotelId);
        }

        public static HotelFacility CreateHotelFacility(string name, Euros unitPrice, long hotelId)
        {
            return new HotelFacility(name, unitPrice, false, hotelId);
        }

        public static RoomFacility CreateFreeRoomFacility(string name, long roomId)
        {
            return new RoomFacility(name, Euros.Of(0), true, roomId);
        }

        public static RoomFacility CreateRoomFacility(string name, Euros unitPrice, long roomId)
        {
            return new RoomFacility(name, unitPrice, false, roomId);
        }
    }
}
