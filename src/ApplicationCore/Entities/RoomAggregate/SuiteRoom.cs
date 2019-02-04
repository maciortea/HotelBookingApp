﻿namespace ApplicationCore.Entities.RoomAggregate
{
    public class SuiteRoom : RoomType
    {
        public const string SuiteRoomName = "Suite";

        public SuiteRoom(long hotelId, Euros pricePerNight)
            : base(hotelId, SuiteRoomName, pricePerNight)
        {
        }
    }
}
