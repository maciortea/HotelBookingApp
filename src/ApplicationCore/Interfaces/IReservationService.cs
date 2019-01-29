using ApplicationCore.Entities.ReservationAggregate;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationService
    {
        Task<Result<IReadOnlyCollection<Reservation>>> ListAllAsync(long hotelId);
        Task<Result> CreateAsync(Reservation reservation);
        Task<Result> CheckoutAsync(long id);
        Task<Result> CancelAsync(long id);
    }
}
