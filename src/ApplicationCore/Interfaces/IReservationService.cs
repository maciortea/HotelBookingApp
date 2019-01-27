using ApplicationCore.Entities.ReservationAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationService
    {
        Task<IReadOnlyCollection<Reservation>> ListAllAsync(long hotelId);
        Task CreateAsync(Reservation reservation);
        Task CancelAsync(long id);
    }
}
