using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using theCarHub.Data;
using theCarHub.Models;

namespace theCarHub.Controllers
{
    public class CarListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarListController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            return user?.Id;
        }

        public async Task<IActionResult> Index()
        {
            var id = await GetCurrentUserId();
            var userCars = _context.UserCars.Where(x => x.UserId == id);
            var model = userCars.Select(x => new CarViewModel
            {
                CarId = x.CarId,
                Name = x.Car.Name,
                Year = x.Car.Year.Year,
                Listed = x.Listed,
                ToShow = true,
                Rating = x.Rating
            }).ToList();

            return View(model);
        }
    }
}