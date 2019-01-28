using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomAggregate;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces
{
    public interface IPriceCalculator
    {
        decimal CalculatePrice(Room room, IReadOnlyCollection<Facility> facilities, int noOfNights);
    }
}
