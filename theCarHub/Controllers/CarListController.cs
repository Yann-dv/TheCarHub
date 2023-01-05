using System.Globalization;
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
                            }));
            foreach (var item in model)
            {
                var userCar = await _context.UserCars.FirstOrDefaultAsync(x =>
                    x.UserId == userId && x.CarId == item.CarId);
                if (userCar != null)
                { 
                    item.CarId = 1;
                }
            }
            return View(model);
        }
        
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return RedirectToAction("Create", "Cars");;
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
        
        [HttpGet]
        public async Task<JsonResult> CarListToggler(int id, int val)
        {
            int retval = -1;
            var userId = await GetCurrentUserId();
            if (val == 1)
            {
                // if a record exists in UserCars that contains both the user’s
                // and car’s Ids, then the car is in the watchlist and can
                // be removed
                var car = _context.UserCars?.FirstOrDefault(uc =>
                    uc.CarId == id && uc.UserId == userId);
                if (car != null)
                {
                    _context.UserCars?.Remove(car);
                    retval = 0;
                }

            }
            else
            {
                // the car is not currently in the watchlist, so we need to
                // build a new UserCar object and add it to the database
                _context.UserCars.Add(
                    new UserCar
                    {
                        UserId = userId,
                        CarId = id,
                        InUserBasket = false
                    }
                );
                retval = 1;
            }
           
            await _context.SaveChangesAsync(); // now we can save the changes to the database
            // and our return value (-1, 0, or 1) back to the script that called
            // this method from the Index page
            return Json(retval);
        }
    }
}