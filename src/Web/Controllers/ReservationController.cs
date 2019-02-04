using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ReservationAggregate;
using ApplicationCore.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.Reservation;

namespace Web.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly UserManager<HotelPersonal> _userManager;
        private readonly IReservationRepository _reservationRepository;
        private readonly IReservationService _reservationService;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IPriceCalculator _priceCalculator;

        public ReservationController(
            UserManager<HotelPersonal> userManager,
            IReservationRepository reservationRepository,
            IReservationService reservationService,
            IHotelRepository hotelRepository,
            IRoomTypeRepository roomTypeRepository,
            IPriceCalculator priceCalculator)
        {
            _userManager = userManager;
            _reservationRepository = reservationRepository;
            _reservationService = reservationService;
            _hotelRepository = hotelRepository;
            _roomTypeRepository = roomTypeRepository;
            _priceCalculator = priceCalculator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with id '{User.Identity.Name}'.");
            }

            if (user.HotelId <= 0)
            {
                throw new ApplicationException($"User with id '{User.Identity.Name}' is not part of any hotel.");
            }

            var reservationsResult = await _reservationService.ListAllAsync(user.HotelId);
            if (reservationsResult.IsFailure)
            {
                throw new ApplicationException(reservationsResult.Error);
            }

            var model = reservationsResult.Value
                .Select(r => new ReservationViewModel(DateTime.Today)
                {
                    Id = r.Id,
                    CustomerFullName = r.Customer.FullName,
                    RoomType = r.Room.RoomType.Type,
                    ReservationPeriod = new ReservationPeriodViewModel(r.CheckinDate, r.CheckoutDate),
                    CreationDate = r.CreationDate,
                    Floor = r.Room.Floor
                })
                .ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult SelectStayPeriod()
        {
            var model = new ReservationPeriodViewModel(DateTime.Now, DateTime.Now.AddDays(1));
            return View(model);
        }

        [HttpPost]
        public IActionResult SelectStayPeriod(ReservationPeriodViewModel model)
        {
            return RedirectToAction("Create", new { CheckinDate = model.CheckinDate.Date, CheckoutDate = model.CheckoutDate.Date });
        }

        [HttpGet]
        public async Task<IActionResult> Create(DateTime checkinDate, DateTime checkoutDate)
        {
            if (checkinDate >= checkoutDate)
            {
                throw new ApplicationException("Checkin-date cannot be same or after check-out date.");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with id '{User.Identity.Name}'.");
            }

            var availableRoomsResult = await _hotelRepository.GetAvailableRoomsByPeriodAsync(user.HotelId, checkinDate, checkoutDate);
            if (availableRoomsResult.IsFailure)
            {
                throw new ApplicationException(availableRoomsResult.Error);
            }

            var hotelFacilitiesResult = await _hotelRepository.GetFacilitiesByHotelIdAsync(user.HotelId);
            if (hotelFacilitiesResult.IsFailure)
            {
                throw new ApplicationException(availableRoomsResult.Error);
            }

            var model = new ReservationEditViewModel
            {
                ReservationPeriod = new ReservationPeriodViewModel(checkinDate, checkoutDate),
                AvailableRoomTypes = availableRoomsResult.Value
                    .GroupBy(r => r.RoomType.Type)
                    .Select(g => new RoomViewModel
                    {
                        Id = g.Select(r => r.Id).First(),
                        Type = g.Key
                    })
                    .ToList(),
                HotelFacilities = hotelFacilitiesResult.Value
                    .Select(f => new FacilityViewModel
                    {
                        Id = f.Id,
                        Name = f.Name
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = new Customer(model.CustomerFirstName, model.CustomerLastName, model.CustomerPhone);
            var reservation = new Reservation(model.RoomId, customer, model.ReservationPeriod.CheckinDate.Date, model.ReservationPeriod.CheckoutDate.Date);
            model.HotelFacilityIds.ForEach(id => reservation.AddReservationFacility(new ReservationFacility(0, id)));
            await _reservationService.CreateAsync(reservation);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(long id)
        {
            var reservationResult = await _reservationRepository.GetFullByIdAsync(id);
            if (reservationResult.IsFailure)
            {
                throw new ApplicationException(reservationResult.Error);
            }

            var roomFacilitiesResult = await _roomTypeRepository.GetFacilitiesByRoomIdAsync(reservationResult.Value.Room.RoomTypeId);
            if (roomFacilitiesResult.IsFailure)
            {
                throw new ApplicationException(roomFacilitiesResult.Error);
            }

            var model = new ReservationCheckoutViewModel
            {
                Id = reservationResult.Value.Id,
                RoomType = reservationResult.Value.Room.RoomType.Type,
                CustomerFullName = reservationResult.Value.Customer.FullName,
                NoOfNights = reservationResult.Value.CalculateCheckoutNoOfNights(DateTime.Today),
                HotelFacilities = reservationResult.Value.Facilities
                    .Select(f => new FacilityViewModel
                    {
                        Id = f.HotelFacilityId,
                        Name = f.HotelFacility.Name,
                        UnitPrice = f.HotelFacility.UnitPrice,
                        FreeOfCharge = f.HotelFacility.FreeOfCharge
                    })
                    .ToList(),
                RoomFacilities = roomFacilitiesResult.Value
                    .Select(f => new FacilityViewModel
                    {
                        Id = f.Id,
                        Name = f.Name,
                        UnitPrice = f.UnitPrice,
                        FreeOfCharge = f.FreeOfCharge
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(ReservationCheckoutViewModel model)
        {
            await _reservationService.CheckoutAsync(model.Id);
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> CalculatePrice(long id, long[] roomFacilityIds)
        {
            var reservationResult = await _reservationRepository.GetFullByIdAsync(id);
            if (reservationResult.IsFailure)
            {
                throw new ApplicationException(reservationResult.Error);
            }

            var roomFacilitiesResult = await _roomTypeRepository.GetFacilitiesByIds(reservationResult.Value.Room.RoomTypeId, roomFacilityIds);
            if (roomFacilitiesResult.IsFailure)
            {
                throw new ApplicationException(roomFacilitiesResult.Error);
            }

            int noOfNights = reservationResult.Value.CalculateCheckoutNoOfNights(DateTime.Today);

            var facilities = new List<Facility>();
            facilities.AddRange(roomFacilitiesResult.Value);
            facilities.AddRange(reservationResult.Value.Facilities.Select(f => f.HotelFacility));
            decimal price = _priceCalculator.CalculatePrice(reservationResult.Value.Room.RoomType, facilities, noOfNights);

            return Json(price);
        }

        [HttpGet]
        public async Task<IActionResult> Cancel(long id)
        {
            await _reservationService.CancelAsync(id);
            return RedirectToAction("List");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}