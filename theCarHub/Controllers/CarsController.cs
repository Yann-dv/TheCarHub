﻿using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using theCarHub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using theCarHub.Models;

namespace theCarHub.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CarsController> _logger;
        private readonly IWebHostEnvironment _environment;

        public CarsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            ILogger<CarsController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _environment = environment;
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
            ApplicationUser user = await GetCurrentUserAsync();
            
            var model = await _context.Cars.Where(c => c.ToSale == true)
                .Select(x =>
                    new CarViewModel
                    {
                        CarId = x.Id,
                        OwnerId = x.OwnerId,
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
                    }).ToListAsync();

            string BaseUrl = "https://thecarhubapi.azurewebsites.net/";
            List<CarImagesNewModel> ListOfImagesUrl = new List<CarImagesNewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/storage/get");
                if (Res.IsSuccessStatusCode)
                {
                    var empResponse = Res.Content.ReadAsStringAsync().Result;
                    ListOfImagesUrl = JsonConvert.DeserializeObject<List<CarImagesNewModel>>(empResponse);
                }
            }

            ViewBag.CurrentUserId = _userManager.GetUserId(HttpContext.User);

            return View(Tuple.Create(model, ListOfImagesUrl));
        }

        public async Task<string> DeleteImage()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            return user?.Id;
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            return RedirectToAction(nameof(Index));
        }


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
            
            ViewBag.CurrentUserId = _userManager.GetUserId(HttpContext.User);

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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
        [Bind("Id, OwnerId, Year, Make, Model, Trim, PurchaseDate, PurchasePrice, Repairs, RepairCost, LotDate, SellingPrice, SaleDate, Description, ToSale")] 
        Car car)
        {
            //car.OwnerId = user.Id;
            
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                car.OwnerId = user.Id;
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "Unable to create the car : the model seems invalid. Try again, and if the problem persists, see your system administrator.");
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
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id, Year, Make, Model, Trim, PurchaseDate, PurchasePrice, Repairs, RepairCost, LotDate, SellingPrice, SaleDate, Description, ToSale")]
            Car car)
        {
            if (id != car.Id || car.OwnerId != _userManager.GetUserId(HttpContext.User))
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
            if (car == null ||  car.OwnerId != _userManager.GetUserId(HttpContext.User))
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
