using ApplicationCore.Entities;
using ApplicationCore.Entities.RoomTypeAggregate;
using System.Collections.Generic;

namespace ApplicationCore.Interfaces
{
    public interface IPriceCalculator
    {
        decimal CalculatePrice(RoomType room, IReadOnlyCollection<Facility> facilities, int noOfNights);
    }
}
