using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IReadOnlyCollection<Reservation>> ListAllAsync(long hotelId)
        {
            return await _reservationRepository.GetAllByHotelIdAsync(hotelId);
        }

        public async Task CreateAsync(Reservation reservation)
        {
            await _reservationRepository.CreateAsync(reservation);
        }

        public async Task CancelAsync(long id)
        {
            Reservation reservation = await _reservationRepository.GetByIdAsync(id);
            if (reservation == null)
            {
                // error
            }

            reservation.Cancel();
            await _reservationRepository.UpdateAsync(reservation);
        }
    }
}
