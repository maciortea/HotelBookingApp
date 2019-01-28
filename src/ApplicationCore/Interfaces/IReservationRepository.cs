using ApplicationCore.Entities.ReservationAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Reservation> GetFullByIdAsync(long id);
        Task<IReadOnlyCollection<Reservation>> GetAllByHotelIdAsync(long hotelId);
        Task<long[]> GetIdsByHotelIdAndPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate);
    }
}
