using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomAggregate;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        private readonly IAppLogger<PriceCalculator> _logger;

        public PriceCalculator(IAppLogger<PriceCalculator> logger)
        {
            _logger = logger;
        }

        public decimal CalculatePrice(Room room, IReadOnlyCollection<Facility> facilities, int noOfNights)
        {
            if (room == null)
            {
                _logger.LogInformation("Room is null");
                return 0m;
            }

            if (facilities == null)
            {
                _logger.LogInformation("Facilities are null");
                return 0m;
            }

            if (noOfNights <= 0)
            {
                _logger.LogInformation("Number of nights is zero or negative");
                return 0m;
            }

            decimal totalPrice = room.PricePerNight * noOfNights;

            List<Facility> chargeableFacilities = facilities.Where(f => !f.FreeOfCharge).ToList();
            if (chargeableFacilities.Count > 0)
            {
                foreach (Facility facility in chargeableFacilities)
                {
                    totalPrice += facility.UnitPrice * noOfNights;
                }
            }

            return totalPrice;
        }
    }
}
