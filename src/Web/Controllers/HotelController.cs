using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.HotelAggregate;
using ApplicationCore.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Hotel;

namespace Web.Controllers
{
    [Authorize]
    public class HotelController : Controller
    {
        private readonly UserManager<HotelPersonal> _userManager;
        private readonly IHotelRepository _hotelRepository;

        public HotelController(UserManager<HotelPersonal> userManager, IHotelRepository hotelRepository)
        {
            _userManager = userManager;
            _hotelRepository = hotelRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with id '{User.Identity.Name}'.");
            }

            
            Hotel hotel = await _hotelRepository.GetFullByIdAsync(user.HotelId);
            var model = new HotelViewModel
            {
                Name = hotel.Name,
                FullAddress = hotel.Address.Display
            };

            return View(model);
        }
    }
}