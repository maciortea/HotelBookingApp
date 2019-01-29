﻿using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReservationRepository : EfRepository<Reservation>, IReservationRepository
    {
        private readonly IAppLogger<ReservationRepository> _logger;

        public ReservationRepository(ApplicationDbContext db, IAppLogger<ReservationRepository> logger) : base(db)
        {
            _logger = logger;
        }

        public async Task<Result<Reservation>> GetFullByIdAsync(long id)
        {
            try
            {
                Reservation reservation = await _db.Reservations
                    .Include(r => r.Facilities).ThenInclude(f => f.HotelFacility)
                    .Include(r => r.RoomItem.Room)
                    .Where(r => r.Id == id)
                    .FirstOrDefaultAsync();

                return Result.Ok(reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<Reservation>(ex.Message);
            }
        }

        public async Task<Result<IReadOnlyCollection<Reservation>>> GetAllByHotelIdAsync(long hotelId)
        {
            try
            {
                IReadOnlyCollection<Reservation> reservations = await _db.Reservations
                    .Include(r => r.RoomItem.Room)
                    .Where(r => r.RoomItem.Room.HotelId == hotelId && !r.CheckedOut && !r.Canceled)
                    .ToListAsync();

                return Result.Ok(reservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<IReadOnlyCollection<Reservation>>(ex.Message);
            }
        }
    }
}
