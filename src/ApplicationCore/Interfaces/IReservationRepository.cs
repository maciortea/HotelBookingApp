using ApplicationCore.Entities.ReservationAggregate;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Result<Reservation>> GetFullByIdAsync(long id);
        Task<Result<IReadOnlyCollection<Reservation>>> GetAllByHotelIdAsync(long hotelId);
    }
}
