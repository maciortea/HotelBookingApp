using ApplicationCore.Entities.ReservationAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(long id);
        Task<IReadOnlyCollection<Reservation>> GetAllByHotelIdAsync(long hotelId);
        Task CreateAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
    }
}
