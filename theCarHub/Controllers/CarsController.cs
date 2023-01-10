using System.Net.Http.Headers;
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
        private readonly ApiController _apiController;

        public CarsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ApiController apiController)
        {
            _context = context;
            _userManager = userManager;
            _apiController = apiController;
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
                        ToSale = x.ToSale
                    }).ToListAsync();
            
            string BaseUrl = "https://thecarhubapi.azurewebsites.net/";
            List<CarImagesNewModel> ListOfImagesUrl = new List<CarImagesNewModel>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/storage/get");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    ListOfImagesUrl = JsonConvert.DeserializeObject<List<CarImagesNewModel>>(EmpResponse);
                }
            }
            return View(Tuple.Create(model,ListOfImagesUrl));
        }
        
        //Get : car images 
        /*public async Task<IActionResult> GetCarImages()
        {
            return RedirectToAction("Indexz", "Api");
        }*/

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
        public async Task<IActionResult> Create(
            [Bind(
                "Id, Year, Make, Model, Trim, PurchaseDate, PurchasePrice, Repairs, RepairCost, LotDate, SellingPrice, SaleDate, Description, ToSale")]
            Car car)
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
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "Id, Year, Make, Model, Trim, PurchaseDate, PurchasePrice, Repairs, RepairCost, LotDate, SellingPrice, SaleDate, Description, ToSale")]
            Car car)
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
