using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IAppLogger<ReservationService> _logger;
        private readonly IRepository<Reservation> _reservationRepository;

        public ReservationService(IAppLogger<ReservationService> logger, IRepository<Reservation> reservationRepository)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<Reservation>>> ListAllAsync(long hotelId)
        {
            try
            {
                var specification = new AllReservationsByHotelIdIncludingRoomTypeSpecification(hotelId);
                IReadOnlyCollection<Reservation> reservations = await _reservationRepository.ListAsync(specification);
                return Result.Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<IReadOnlyCollection<Reservation>>(ex.Message);
            }
        }

        public async Task<Result<Reservation>> GetFullByIdAsync(long id)
        {
            try
            {
                var specification = new ReservationWithFullMembersSpecification(id);
                Reservation reservation = await _reservationRepository.FirstOrDefaultAsync(specification);
                if (reservation == null)
                {
                    string message = $"Reservation with id '{id}' doesn't exists";
                    _logger.LogInformation(message);
                    return Result.Fail<Reservation>(message);
                }

                return Result.Ok(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<Reservation>(ex.Message);
            }
        }

        public async Task<Result> CreateAsync(Reservation reservation)
        {
            try
            {
                await _reservationRepository.AddAsync(reservation);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> CheckoutAsync(long id)
        {
            try
            {
                Reservation reservation = await _reservationRepository.GetByIdAsync(id);
                if (reservation == null)
                {
                    return Result.Fail($"Reservation with id '{id}' doesn't exists");
                }

                reservation.Checkout();
                await _reservationRepository.UpdateAsync(reservation);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> CancelAsync(long id)
        {
            try
            {
                Reservation reservation = await _reservationRepository.GetByIdAsync(id);
                if (reservation == null)
                {
                    return Result.Fail($"Reservation with id '{id}' doesn't exists");
                }

                reservation.Cancel();
                await _reservationRepository.UpdateAsync(reservation);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
