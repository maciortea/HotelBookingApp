using ApplicationCore.Entities.ReservationAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationRepository
    {
        Task<IReadOnlyCollection<Reservation>> GetAllByHotelId(long hotelId);
        Task Create(Reservation reservation);
    }
}
