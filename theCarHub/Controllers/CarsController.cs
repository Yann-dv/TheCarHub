using System.Net.Http.Headers;
using System.Text;
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

        public CarsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CarsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/storage/get");
                if (Res.IsSuccessStatusCode)
                {
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    ListOfImagesUrl = JsonConvert.DeserializeObject<List<CarImagesNewModel>>(EmpResponse);
                }
            }
            return View(Tuple.Create(model,ListOfImagesUrl));
        }
        
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload (IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || string.IsNullOrEmpty(file?.ContentType))
                return BadRequest();
            return Ok();
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
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
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    ListOfImagesUrl = JsonConvert.DeserializeObject<List<CarImagesNewModel>>(EmpResponse);
                }
            }

            return View(nameof(Index));
        }
        
        
        /*[HttpPost("UploadFilxxe")]
        public IActionResult UploadFilexxx(IFormFile formFile)
        {
            _logger.LogInformation("UploadFile" + formFile.FileName);
            return Ok(formFile.FileName);
        }

        [HttpPost("UploadFilexx")]
        private static async Task UploadFilexx()
        {
            string filePath = Path.GetFullPath();
            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
    
            // here it is important that second parameter matches with name given in API.
            form.Add(fileContent, "formFile", Path.GetFileName(filePath));

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://thecarhubapi.azurewebsites.net/")
            };

            string uri = "";
            var response = await httpClient.PostAsync($"/api/storage/post", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("response :" + responseContent);
        }*/


        /*

        string path = "https://thecarhubapi.azurewebsites.net/"
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFiles.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFiles.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }
            return View();*/

        public async Task<string> DeleteImage()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            return user?.Id;
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
