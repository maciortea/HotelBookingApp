﻿using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : Repository, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task<IReadOnlyCollection<Reservation>> GetAllByHotelId(long hotelId)
        {
            return await _db.Reservations.Include(r => r.Room).Where(r => r.Room.HotelId == hotelId).ToListAsync();
        }

        public async Task Create(Reservation reservation)
        {
            await _db.Reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();
        }
    }
}