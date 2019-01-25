using ApplicationCore.Entities.ReservationAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationService
    {
        Task<IReadOnlyCollection<Reservation>> ListAll(long hotelId);
        Task Create(Reservation reservation);
    }
}
