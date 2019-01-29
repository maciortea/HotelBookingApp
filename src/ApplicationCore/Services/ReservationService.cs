using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IAppLogger<ReservationService> _logger;
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IAppLogger<ReservationService> logger, IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
            _logger = logger;
        }

        public async Task<Result<IReadOnlyCollection<Reservation>>> ListAllAsync(long hotelId)
        {
            return await _reservationRepository.GetAllByHotelIdAsync(hotelId);
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
