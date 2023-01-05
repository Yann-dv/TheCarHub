using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using theCarHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using theCarHub.Models;

namespace theCarHub.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<string> GetCurrentUserId()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            return user?.Id;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() =>
            _userManager.GetUserAsync(HttpContext.User);

        // GET: Cars
        public async Task<IActionResult> Index()
        {

            /*var userId = await GetCurrentUserId();
            var userCars = _context.UserCars.Where(x => x.UserId == userId);
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
            }).ToList();
            
            foreach (var item in model)
            {
                var userCar = await _context.UserCars.FirstOrDefaultAsync(x =>
                    x.UserId == userId && x.CarId == item.CarId);
                if (userCar != null)
                { 
                    item.ToSale = true;
                }
            }*/
            var model = await _context.Cars.Where(c => c.ToSale == true)
                .Select(x =>
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
                }).ToListAsync();

            return View(model);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Year, Make, Model, Trim, PurchaseDate, PurchasePrice, Repairs, RepairCost, LotDate, SellingPrice, SaleDate, Description")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }
        
        [Authorize(Roles = "Admin")]
        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Year, Make, Model, Trim, PurchaseDate, PurchasePrice, Repairs, RepairCost, LotDate, SellingPrice, SaleDate, Description")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        [Authorize(Roles = "Admin")]
        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
        
        [Authorize(Roles = "Admin")]
        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
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
