using ApplicationCore.Entities.ReservationAggregate;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationService
    {
        Task<IReadOnlyCollection<Reservation>> ListAllAsync(long hotelId);
        Task<Result<Reservation>> GetFullByIdAsync(long id);
        Task<Result> CreateAsync(Reservation reservation);
        Task<Result> CheckoutAsync(long id);
        Task<Result> CancelAsync(long id);
    }
}
