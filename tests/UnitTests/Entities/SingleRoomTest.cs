using ApplicationCore.Common;
using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomAggregate;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class SingleRoomTest
    {
        [Fact]
        public void Constructor_WithInvalidArguments_ShouldThrowContractException()
        {
            Func<SingleRoom> createRoom1 = () => new SingleRoom(-1, Euros.Of(10m));
            Func<SingleRoom> createRoom2 = () => new SingleRoom(0, Euros.Of(10m));
            Assert.Throws<ContractException>(createRoom1);
            Assert.Throws<ContractException>(createRoom2);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var room = new SingleRoom(1, Euros.Of(10m));
            Assert.Equal(1, room.HotelId);
            Assert.Equal(SingleRoom.SingleRoomName, room.Type);
            Assert.Equal(Euros.Of(10m), room.PricePerNight);
        }
    }
}
