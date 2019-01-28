using ApplicationCore.Entities.ReservationAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(long id);
        Task<IReadOnlyCollection<Reservation>> GetAllByHotelIdAsync(long hotelId);
        Task<long[]> GetIdsByHotelIdAndPeriodAsync(long hotelId, DateTime checkinDate, DateTime checkoutDate);
        Task CreateAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
    }
}
