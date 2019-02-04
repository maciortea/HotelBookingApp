using ApplicationCore.Common;
using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomTypeAggregate;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class StandardRoomTest
    {
        [Fact]
        public void Constructor_WithInvalidArguments_ShouldThrowContractException()
        {
            Func<StandardRoom> createRoom1 = () => new StandardRoom(-1, Euros.Of(10m));
            Func<StandardRoom> createRoom2 = () => new StandardRoom(0, Euros.Of(10m));
            Assert.Throws<ContractException>(createRoom1);
            Assert.Throws<ContractException>(createRoom2);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var room = new StandardRoom(1, Euros.Of(10m));
            Assert.Equal(1, room.HotelId);
            Assert.Equal(StandardRoom.StandardRoomName, room.Type);
            Assert.Equal(Euros.Of(10m), room.PricePerNight);
        }
    }
}
