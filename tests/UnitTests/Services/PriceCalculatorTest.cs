using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomAggregate;
using ApplicationCore.Factories;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests.Services
{
    public class PriceCalculatorTest
    {
        private readonly Mock<IAppLogger<PriceCalculator>> _loggerMock;
        private readonly IPriceCalculator _priceCalculator;

        public PriceCalculatorTest()
        {
            _loggerMock = new Mock<IAppLogger<PriceCalculator>>();
            _priceCalculator = new PriceCalculator(_loggerMock.Object);
        }

        [Fact]
        public void CalculatePrice_WithNullRoom_ShouldLogInfoAndReturnZeroPrice()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var facilities = GetFacilities();
            decimal price = _priceCalculator.CalculatePrice(null, facilities, 2);

            Assert.Equal(0m, price);
            _loggerMock.Verify(x => x.LogInformation("Room is null"), Times.Once);
        }

        [Fact]
        public void CalculatePrice_WithNullFacilities_ShouldLogInfoAndReturnZeroPrice()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            decimal price = _priceCalculator.CalculatePrice(room, null, 2);

            Assert.Equal(0m, price);
            _loggerMock.Verify(x => x.LogInformation("Facilities are null"), Times.Once);
        }

        [Fact]
        public void CalculatePrice_WithNegativeNoOfNights_ShouldLogInfoAndReturnZeroPrice()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            var facilities = GetFacilities();
            decimal price = _priceCalculator.CalculatePrice(room, facilities, -1);

            Assert.Equal(0m, price);
            _loggerMock.Verify(x => x.LogInformation("Number of nights is zero or negative"), Times.Once);
        }

        [Fact]
        public void CalculatePrice_WithZeroNoOfNights_ShouldLogInfoAndReturnZeroPrice()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            var facilities = GetFacilities();
            decimal price = _priceCalculator.CalculatePrice(room, facilities, 0);

            Assert.Equal(0m, price);
            _loggerMock.Verify(x => x.LogInformation("Number of nights is zero or negative"), Times.Once);
        }

        [Fact]
        public void CalculatePrice_WithRoomPrice45NoFacilitiesAndOneNight_ShouldLogInfoAndReturn45()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            var facilities = new List<Facility>();
            decimal price = _priceCalculator.CalculatePrice(room, facilities, 1);

            Assert.Equal(45m, price);
        }

        [Fact]
        public void CalculatePrice_WithRoomPrice45NoFacilitiesAndThreeNights_ShouldLogInfoAndReturn135()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            var facilities = new List<Facility>();
            decimal price = _priceCalculator.CalculatePrice(room, facilities, 3);

            Assert.Equal(135m, price);
        }

        [Fact]
        public void CalculatePrice_WithRoomPrice45TwoFacilitiesFor5AndOneNight_ShouldLogInfoAndReturn55()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            var facilities = GetFacilities();
            decimal price = _priceCalculator.CalculatePrice(room, facilities, 1);

            Assert.Equal(55m, price);
        }

        [Fact]
        public void CalculatePrice_WithRoomPrice45TwoFacilitiesFor5AndThreeNights_ShouldLogInfoAndReturn165()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            var facilities = GetFacilities();
            decimal price = _priceCalculator.CalculatePrice(room, facilities, 3);

            Assert.Equal(165m, price);
        }

        [Fact]
        public void CalculatePrice_WithRoomPrice45NotChargeableFacilitiesAndThreeNights_ShouldLogInfoAndReturn135()
        {
            _loggerMock.Setup(x => x.LogInformation(It.IsAny<string>()));

            var room = new Room(1, "type", Euros.Of(45));
            var facilities = GetNotChargeableFacilities();
            decimal price = _priceCalculator.CalculatePrice(room, facilities, 3);

            Assert.Equal(135m, price);
        }

        private IReadOnlyCollection<Facility> GetFacilities()
        {
            return new List<Facility>
            {
                FacilityFactory.CreateHotelFacility("facility1", Euros.Of(5), 1),
                FacilityFactory.CreateRoomFacility("facility2", Euros.Of(5), 2),
                FacilityFactory.CreateFreeHotelFacility("facility3-free", 1),
                FacilityFactory.CreateFreeRoomFacility("facility4-free", 2)
            };
        }

        private IReadOnlyCollection<Facility> GetNotChargeableFacilities()
        {
            return new List<Facility>
            {
                FacilityFactory.CreateFreeHotelFacility("facility1-free", 1),
                FacilityFactory.CreateFreeRoomFacility("facility2-free", 2)
            };
        }
    }
}
