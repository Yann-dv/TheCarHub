using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using theCarHub.Data;
using theCarHub.Models;

namespace theCarHub.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WatchlistController(ApplicationDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<AppUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            AppUser user = await GetCurrentUserAsync();
            return user?.Id;
        }

        public async Task<IActionResult> Index()
        {
            var id = await GetCurrentUserId();
            var userMovies = _context.UserCars.Where(x => x.UserId == id);
            var model = userMovies.Select(x => new CarViewModel
            {
                CarId = x.CarId,
                Name = x.Car.Name,
                Year = x.Car.Year,
                Watched = x.Watched,
                InWatchlist = true,
                Rating = x.Rating
            }).ToList();

            return View(model);
        }
    }
}