using ApplicationCore.Common;
using ApplicationCore.Entities.HotelAggregate;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class RoomItemTest
    {
        [Fact]
        public void Constructor_WithInvalidArguments_ShouldThrowContractException()
        {
            Func<Room> createRoomItem1 = () => new Room(-1, 1, 1);
            Func<Room> createRoomItem2 = () => new Room(0, 1, 1);
            Func<Room> createRoomItem3 = () => new Room(1, -1, 1);
            Func<Room> createRoomItem4 = () => new Room(1, 11, 1);
            Func<Room> createRoomItem5 = () => new Room(1, 10, -1);
            Func<Room> createRoomItem6 = () => new Room(1, 10, 0);
            Assert.Throws<ContractException>(createRoomItem1);
            Assert.Throws<ContractException>(createRoomItem2);
            Assert.Throws<ContractException>(createRoomItem3);
            Assert.Throws<ContractException>(createRoomItem4);
            Assert.Throws<ContractException>(createRoomItem5);
            Assert.Throws<ContractException>(createRoomItem6);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var roomItem = new Room(1, 0, 1);
            Assert.Equal(1, roomItem.RoomTypeId);
            Assert.Equal(0, roomItem.Floor);
            Assert.Equal(1, roomItem.Number);
        }
    }
}
