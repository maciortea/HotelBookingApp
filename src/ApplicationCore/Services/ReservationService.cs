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

        public async Task<IReadOnlyCollection<Reservation>> ListAll(long hotelId)
        {
            return await _reservationRepository.GetAllByHotelId(hotelId);
        }

        public async Task Create(Reservation reservation)
        {
            await _reservationRepository.Create(reservation);
        }
    }
}
