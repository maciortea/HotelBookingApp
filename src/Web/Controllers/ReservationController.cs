using System;
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
        private readonly IRoomRepository _roomRepository;

        public ReservationController(
            UserManager<HotelPersonal> userManager,
            IReservationRepository reservationRepository,
            IReservationService reservationService,
            IHotelRepository hotelRepository,
            IRoomRepository roomRepository)
        {
            _userManager = userManager;
            _reservationRepository = reservationRepository;
            _reservationService = reservationService;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                // show error
            }

            var reservations = await _reservationService.ListAllAsync(user.HotelId);
            var model = reservations
                .Select(r => new ReservationViewModel(DateTime.Today)
                {
                    Id = r.Id,
                    CustomerFullName = r.Customer.FullName,
                    RoomType = r.RoomItem.Room.Type,
                    ReservationPeriod = new ReservationPeriodViewModel(r.CheckinDate, r.CheckoutDate),
                    CreationDate = r.CreationDate
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
            return RedirectToAction("Create", new { model.CheckinDate, model.CheckoutDate });
        }

        [HttpGet]
        public async Task<IActionResult> Create(DateTime checkinDate, DateTime checkoutDate)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                // show error
            }

            var availableRooms = await _hotelRepository.GetAvailableRoomsByPeriodAsync(user.HotelId, checkinDate, checkoutDate);
            var hotelFacilities = await _hotelRepository.GetFacilitiesByHotelIdAsync(user.HotelId);

            var model = new ReservationEditViewModel
            {
                ReservationPeriod = new ReservationPeriodViewModel(checkinDate, checkoutDate),
                AvailableRooms = availableRooms
                    .GroupBy(r => r.Room.Type)
                    .Select(g => new RoomViewModel
                    {
                        Id = g.Select(r => r.Id).First(),
                        Type = g.Key
                    })
                    .ToList(),
                HotelFacilities = hotelFacilities
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
            var reservation = new Reservation(model.RoomId, customer, model.ReservationPeriod.CheckinDate, model.ReservationPeriod.CheckoutDate);
            model.HotelFacilityIds.ForEach(id => reservation.AddReservationFacility(new ReservationFacility(0, id)));
            await _reservationService.CreateAsync(reservation);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(long id)
        {
            Reservation reservation = await _reservationRepository.GetFullByIdAsync(id);
            if (reservation == null)
            {
                // show error
            }

            var roomFacilities = await _roomRepository.GetAllByRoomIdAsync(reservation.RoomItem.RoomId);

            var model = new ReservationCheckoutViewModel
            {
                Id = reservation.Id,
                RoomType = reservation.RoomItem.Room.Type,
                CustomerFullName = reservation.Customer.FullName,
                NoOfNights = (DateTime.Today - reservation.CheckinDate).Days,
                HotelFacilities = reservation.Facilities.Select(f => f.HotelFacility.Name).ToList(),
                RoomFacilities = roomFacilities
                    .Select(f => new FacilityViewModel
                    {
                        Id = f.Id,
                        Name = f.Name
                    })
                    .ToList()
            };

            if (reservation.CheckoutDate > DateTime.Today)
            {
                model.NoOfNights = Math.Abs((DateTime.Today - reservation.CheckinDate).Days - 1);
            }
            else
            {
                model.NoOfNights = reservation.NoOfNights;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Checkout(ReservationCheckoutViewModel model)
        {
            throw new NotImplementedException();
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