using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IReservationService _reservationService;
        private readonly IRoomRepository _roomRepository;

        public ReservationController(UserManager<HotelPersonal> userManager, IReservationService reservationService, IRoomRepository roomRepository)
        {
            _userManager = userManager;
            _reservationService = reservationService;
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

            var reservations = await _reservationService.ListAll(user.HotelId);
            var model = reservations
                .Select(r => new ReservationViewModel(DateTime.Today)
                {
                    Id = r.Id,
                    CustomerFullName = r.Customer.FullName,
                    RoomType = r.Room.Type,
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

            var availableRooms = await _roomRepository.GetAvailableByHotelIdAndPeriod(user.HotelId, checkinDate, checkoutDate);

            var model = new ReservationEditViewModel
            {
                ReservationPeriod = new ReservationPeriodViewModel(checkinDate, checkoutDate),
                AvailableRooms = availableRooms
                    .GroupBy(r => r.Type)
                    .Select(g => new RoomViewModel
                    {
                        Id = g.Select(r => r.Id).First(),
                        Type = g.Key
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPut]
        public IActionResult Checkout(long id)
        {
            throw new NotImplementedException();
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
            await _reservationService.Create(reservation);

            return RedirectToAction("List");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}