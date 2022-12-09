using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using theCarHub.Models;
using theCarHub.Data;

namespace theCarHub.Controllers
{

    public class CarImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarImagesController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET
        public async Task<IActionResult> Index()
        {
            var model = await _context.CarImages.Select(x =>
                new CarImagesModel()
                {
                    ImageId = x.Id,
                    CarId = x.CarId,
                    Url = x.ImageUrl,
                }).ToListAsync();

            foreach (var item in model)
            {
                var carImages = await _context.CarImages.FirstOrDefaultAsync(x =>
                    x.CarId == item.CarId);
                if (carImages != null)
                {
                    item.Url = carImages.ImageUrl;
                    item.CarId = carImages.CarId;
                    item.ImageId = carImages.Id;
                }
            }

            return View(model);
        }
    }
}