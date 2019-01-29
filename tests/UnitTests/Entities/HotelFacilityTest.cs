using ApplicationCore.Common;
using ApplicationCore.Entities;
using System;
using Xunit;

namespace UnitTests.Entities
{
    public class HotelFacilityTest
    {
        [Fact]
        public void Constructor_WithNegativeOrZeroHotelId_ShouldThrowContractException()
        {
            Func<HotelFacility> createHotelFacility1 = () => new HotelFacility("name", Euros.Of(5m), true, -1);
            Func<HotelFacility> createHotelFacility2 = () => new HotelFacility("name", Euros.Of(5m), true, 0);
            Assert.Throws<ContractException>(createHotelFacility1);
            Assert.Throws<ContractException>(createHotelFacility2);
        }

        [Fact]
        public void Constructor_WithEmptyName_ShouldThrowContractException()
        {
            Func<HotelFacility> createHotelFacility = () => new HotelFacility("", Euros.Of(5m), true, 1);
            Assert.Throws<ContractException>(createHotelFacility);
        }

        [Fact]
        public void Constructor_WithValidArguments_ShouldInitializeAllFields()
        {
            var hotelFacility = new HotelFacility("name", Euros.Of(5m), true, 1);
            Assert.Equal("name", hotelFacility.Name);
            Assert.Equal(Euros.Of(5m), hotelFacility.UnitPrice);
            Assert.True(hotelFacility.FreeOfCharge);
            Assert.Equal(1, hotelFacility.HotelId);
        }
    }
}
