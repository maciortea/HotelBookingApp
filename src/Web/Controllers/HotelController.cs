using ApplicationCore.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Hotel;

namespace Web.Controllers
{
    [Authorize]
    public class HotelController : Controller
    {
        private readonly UserManager<HotelPersonal> _userManager;
        private readonly IHotelRepository _hotelRepository;
        private readonly IHotelService _hotelService;

        public HotelController(
            UserManager<HotelPersonal> userManager,
            IHotelRepository hotelRepository,
            IHotelService hotelService)
        {
            _userManager = userManager;
            _hotelRepository = hotelRepository;
            _hotelService = hotelService;
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
            
            var hotelResult = await _hotelService.GetFullByIdAsync(user.HotelId);
            if (hotelResult.IsFailure)
            {
                throw new ApplicationException(hotelResult.Error);
            }

            var roomTypesToCountAndPrice = await _hotelRepository.GetRoomTypesToCountAndPrice(user.HotelId);

            var model = new HotelViewModel
            {
                Name = hotelResult.Value.Name,
                FullAddress = hotelResult.Value.Address.Display,
                Facilities = hotelResult.Value.Facilities.Select(f => f.Name).ToList(),
                RoomTypesToCountAndPrice = roomTypesToCountAndPrice
            };

            return View(model);
        }
    }
}