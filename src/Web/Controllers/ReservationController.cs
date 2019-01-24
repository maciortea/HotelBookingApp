using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Reservation;

namespace Web.Controllers
{
    [Authorize]
    [Route("")]
    public class ReservationController : Controller
    {
        private readonly UserManager<HotelPersonal> _userManager;
        private readonly IReservationService _reservationService;

        public ReservationController(UserManager<HotelPersonal> userManager, IReservationService reservationService)
        {
            _userManager = userManager;
            _reservationService = reservationService;
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
                .Select(r => new ReservationViewModel
                {
                    RoomType = r.Room.Type,
                    ReservationDate = r.ReservationDate
                })
                .ToList();

            return View(model);
        }
    }
}