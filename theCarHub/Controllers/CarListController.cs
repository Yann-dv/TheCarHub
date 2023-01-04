using System.Globalization;
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
                Year = x.Car.Year,
                Brand = x.Car.Make,
                Model = x.Car.Model,
                Trim = x.Car.Trim,
                PurchaseDate = x.Car.PurchaseDate,
                PurchasePrice = x.Car.PurchasePrice,
                Repairs = x.Car.Repairs,
                RepairCost = x.Car.RepairCost,
                LotDate = x.Car.LotDate,
                SellingPrice = x.Car.SellingPrice,
                SaleDate = x.Car.SaleDate,
                Description = x.Car.Description,
                Listed = x.Listed,
                ToShow = true,
                Rating = x.Rating
            }).ToList();

            return View(model);
        }
    }
}