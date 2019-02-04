using ApplicationCore.Common;
using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomTypeAggregate;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class SuiteRoomTest
    {
        [Fact]
        public void Constructor_WithInvalidArguments_ShouldThrowContractException()
        {
            Func<SuiteRoom> createRoom1 = () => new SuiteRoom(-1, Euros.Of(10m));
            Func<SuiteRoom> createRoom2 = () => new SuiteRoom(0, Euros.Of(10m));
            Assert.Throws<ContractException>(createRoom1);
            Assert.Throws<ContractException>(createRoom2);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var room = new SuiteRoom(1, Euros.Of(10m));
            Assert.Equal(1, room.HotelId);
            Assert.Equal(SuiteRoom.SuiteRoomName, room.Type);
            Assert.Equal(Euros.Of(10m), room.PricePerNight);
        }
    }
}
