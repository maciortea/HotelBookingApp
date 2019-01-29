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
            Func<RoomItem> createRoomItem1 = () => new RoomItem(-1, 1, 1);
            Func<RoomItem> createRoomItem2 = () => new RoomItem(0, 1, 1);
            Func<RoomItem> createRoomItem3 = () => new RoomItem(1, -1, 1);
            Func<RoomItem> createRoomItem4 = () => new RoomItem(1, 11, 1);
            Func<RoomItem> createRoomItem5 = () => new RoomItem(1, 10, -1);
            Func<RoomItem> createRoomItem6 = () => new RoomItem(1, 10, 0);
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
            var roomItem = new RoomItem(1, 0, 1);
            Assert.Equal(1, roomItem.RoomId);
            Assert.Equal(0, roomItem.Floor);
            Assert.Equal(1, roomItem.Number);
        }
    }
}
