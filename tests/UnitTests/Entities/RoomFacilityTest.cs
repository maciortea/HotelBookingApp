using ApplicationCore.Common;
using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomAggregate;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class RoomFacilityTest
    {
        [Fact]
        public void Constructor_WithNegativeOrZeroHotelId_ShouldThrowContractException()
        {
            Func<RoomFacility> createRoomFacility1 = () => new RoomFacility("name", Euros.Of(5m), true, -1);
            Func<RoomFacility> createRoomFacility2 = () => new RoomFacility("name", Euros.Of(5m), true, 0);
            Assert.Throws<ContractException>(createRoomFacility1);
            Assert.Throws<ContractException>(createRoomFacility2);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowContractException()
        {
            Func<RoomFacility> createRoomFacility = () => new RoomFacility("", Euros.Of(5m), true, 1);
            Assert.Throws<ContractException>(createRoomFacility);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var roomFacility = new RoomFacility("name", Euros.Of(5m), true, 1);
            Assert.Equal("name", roomFacility.Name);
            Assert.Equal(Euros.Of(5m), roomFacility.UnitPrice);
            Assert.True(roomFacility.FreeOfCharge);
            Assert.Equal(1, roomFacility.RoomId);
        }
    }
}
