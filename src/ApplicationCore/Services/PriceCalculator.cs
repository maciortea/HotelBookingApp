using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomAggregate;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Services
{
    public class PriceCalculator : IPriceCalculator
    {
        public decimal CalculatePrice(Room room, IReadOnlyCollection<Facility> facilities, int noOfNights)
        {
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
