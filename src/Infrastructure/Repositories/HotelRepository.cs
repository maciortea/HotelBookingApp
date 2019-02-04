﻿using ApplicationCore.Entities;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class HotelRepository : EfRepository<Hotel>, IHotelRepository
    {
        private readonly IAppLogger<HotelRepository> _logger;

        public HotelRepository(ApplicationDbContext db, IAppLogger<HotelRepository> logger) : base(db)
        {
            _logger = logger;
        }

        public async Task<Hotel> GetFullByIdAsync(long id)
        {
            return await _db.Hotels.Include(h => h.Facilities).Include(h => h.Rooms).Where(h => h.Id == id).SingleOrDefaultAsync();
        }

        public async Task<Result<List<Room>>> GetAvailableRoomsByPeriodAsync(long hotelId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                long[] reservedRoomItemIds = await GetBookedRoomItemIdsByHotelIdAndPeriodAsync(hotelId, fromDate, toDate);
                Hotel hotel = await _db.Hotels.Include(h => h.Rooms).ThenInclude(r => r.RoomType).SingleOrDefaultAsync(h => h.Id == hotelId);
                if (hotel == null)
                {
                    string message = $"Hote with id '{hotelId}' doesn't exists";
                    _logger.LogInformation(message);
                    return Result.Fail<List<Room>>(message);
                }

                List<Room> rooms = hotel.Rooms.Where(r => !reservedRoomItemIds.Contains(r.Id)).OrderBy(r => r.Number).ToList();
                return Result.Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<List<Room>>(ex.Message);
            }
        }

        public async Task<Result<IReadOnlyCollection<HotelFacility>>> GetFacilitiesByHotelIdAsync(long hotelId)
        {
            try
            {
                Hotel hotel = await _db.Hotels.Include(h => h.Facilities).SingleOrDefaultAsync(h => h.Id == hotelId);
                if (hotel == null)
                {
                    string message = $"Hote with id '{hotelId}' doesn't exists";
                    _logger.LogInformation(message);
                    return Result.Fail<IReadOnlyCollection<HotelFacility>>(message);
                }

                return Result.Ok(hotel.Facilities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Result.Fail<IReadOnlyCollection<HotelFacility>>(ex.Message);
            }
        }

        public async Task<Dictionary<string, Tuple<int, decimal>>> GetRoomTypesToCountAndPrice(long hotelId)
        {
            return await _db.Hotels
                .Where(h => h.Id == hotelId)
                .SelectMany(h => h.Rooms)
                .GroupBy(r => new { r.RoomType.Type })
                .Select(r => new
                {
                    Type = r.Key,
                    Count = r.Count(),
                    PricePerNight = r.Select(x => x.RoomType.PricePerNight).FirstOrDefault()
                })
                .ToDictionaryAsync(k => k.Type.Type, v => Tuple.Create<int, decimal>(v.Count, v.PricePerNight));
        }

        private async Task<long[]> GetBookedRoomItemIdsByHotelIdAndPeriodAsync(long hotelId, DateTime fromDate, DateTime toDate)
        {
            return await _db.Reservations
                .Where(r =>
                    r.Room.RoomType.HotelId == hotelId &&
                    (r.CheckinDate >= fromDate && r.CheckinDate < toDate ||
                     r.CheckinDate < fromDate && r.CheckoutDate > fromDate) &&
                    !r.CheckedOut &&
                    !r.Canceled)
                .Select(r => r.RoomId)
                .ToArrayAsync();
        }
    }
}
