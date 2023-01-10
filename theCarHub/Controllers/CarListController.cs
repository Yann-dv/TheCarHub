using System.Globalization;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var userId = await GetCurrentUserId();
            var model = await EntityFrameworkQueryableExtensions.ToListAsync(_context.Cars.Select(x =>
                new CarViewModel
                {
                    CarId = x.Id,
                    Year = x.Year,
                    Brand = x.Make,
                    Model = x.Model,
                    Trim = x.Trim,
                    PurchaseDate = x.PurchaseDate,
                    PurchasePrice = x.PurchasePrice,
                    Repairs = x.Repairs,
                    RepairCost = x.RepairCost,
                    LotDate = x.LotDate,
                    SellingPrice = x.SellingPrice,
                    SaleDate = x.SaleDate,
                    Description = x.Description,
                    ToSale = x.ToSale
                }));
            foreach (var item in model)
            {
                var userCar = await _context.UserCars.FirstOrDefaultAsync(x =>
                    x.UserId == userId && x.CarId == item.CarId);
                if (userCar != null)
                {
                    item.ToSale = true;
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return RedirectToAction("Create", "Cars");
            ;
        }

        public async Task<IActionResult> Details(int? id)
        {
            return RedirectToAction("Details", "Cars", new { id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            return RedirectToAction("Edit", "Cars", new { id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            return RedirectToAction("Delete", "Cars", new { id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SoldToggler(int id, bool toSaleValue)
        {
            var car = _context.Cars?.FirstOrDefault(c => c.Id == id);

            if (car != null)
            {
                switch (toSaleValue)
                {
                    case true:
                        car.ToSale = false;
                        break;
                    case false:
                        car.ToSale = true;
                        break;
                }

                _context.Cars.Update(car);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}