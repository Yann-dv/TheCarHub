﻿using System.Globalization;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using theCarHub.Data;
using theCarHub.Models;

namespace theCarHub.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Index(string sortOrder)
        {
            var currentUserId = await GetCurrentUserId();
            List<CarViewModel> model = null;

            if (User.IsInRole("SuperAdmin"))
            {
                model = await EntityFrameworkQueryableExtensions.ToListAsync(_context.Cars
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
                        }));
            }

            else
            { 
                model = await EntityFrameworkQueryableExtensions.ToListAsync(_context.Cars
                    .Where(c => c.OwnerId == currentUserId)
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
                        }));
            
                foreach (var item in model)
                {
                    var userCar = await _context.UserCars.FirstOrDefaultAsync(x =>
                        x.UserId == currentUserId && x.CarId == item.CarId);
                    if (userCar != null)
                    {
                        item.ToSale = true;
                    }
                }
            }
            
            ViewBag.CarIdSortParm = String.IsNullOrEmpty(sortOrder) ? "CarId_desc" : "";
            ViewBag.YearSortParm = sortOrder == "Year" ? "Year_desc" : "Year";
            ViewBag.BrandSortParm = sortOrder == "Brand" ? "Brand_desc" : "Brand";
            ViewBag.ModelSortParm = sortOrder == "Model" ? "Model_desc" : "Model";
            ViewBag.TrimSortParm = sortOrder == "Trim" ? "Trim_desc" : "Trim";
            ViewBag.PurchaseDateSortParm = sortOrder == "PurchaseDate" ? "PurchaseDate_desc" : "PurchaseDate";
            ViewBag.PurchasePriceSortParm = sortOrder == "PurchasePrice" ? "PurchasePrice_desc" : "PurchasePrice";
            ViewBag.RepairCostSortParm = sortOrder == "RepairCost" ? "RepairCost_desc" : "RepairCost";
            ViewBag.SellingPriceSortParm = sortOrder == "SellingPrice" ? "SellingPrice_desc" : "SellingPrice";
            ViewBag.SaleDateSortParm = sortOrder == "SaleDate" ? "SaleDate_desc" : "SaleDate";
            ViewBag.ToSaleSortParm = sortOrder == "ToSale" ? "ToSale_desc" : "ToSale";
            
            var modelToSort = from m in model select m;
            switch (sortOrder)
            {
                case "CarId_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.CarId);
                    break;
                case "Year":
                    modelToSort = modelToSort.OrderBy(m => m.Year.Year);
                    break;
                case "Year_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.Year.Year);
                    break;
                case "Brand":
                    modelToSort = modelToSort.OrderBy(m => m.Brand);
                    break;
                case "Brand_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.Brand);
                    break;
                case "Model":
                    modelToSort = modelToSort.OrderBy(m => m.Model);
                    break;
                case "Model_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.Model);
                    break;
                case "Trim":
                    modelToSort = modelToSort.OrderBy(m => m.Trim);
                    break;
                case "Trim_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.Trim);
                    break;
                case "PurchaseDate":
                    modelToSort = modelToSort.OrderBy(m => m.PurchaseDate);
                    break;
                case "PurchaseDate_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.PurchaseDate);
                    break;
                case "PurchasePrice":
                    modelToSort = modelToSort.OrderBy(m => m.PurchasePrice);
                    break;
                case "PurchasePrice_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.PurchasePrice);
                    break;
                case "RepairCost":
                    modelToSort = modelToSort.OrderBy(m => m.RepairCost);
                    break;
                case "RepairCost_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.RepairCost);
                    break;
                case "SellingPrice":
                    modelToSort = modelToSort.OrderBy(m => m.SellingPrice);
                    break;
                case "SellingPrice_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.SellingPrice);
                    break;
                case "SaleDate":
                    modelToSort = modelToSort.OrderBy(m => m.SaleDate);
                    break;
                case "SaleDate_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.SaleDate);
                    break;
                case "ToSale":
                    modelToSort = modelToSort.OrderBy(m => m.ToSale);
                    break;
                case "ToSale_desc":
                    modelToSort = modelToSort.OrderByDescending(m => m.ToSale);
                    break;
                default:
                    modelToSort = modelToSort.OrderBy(m => m.SaleDate);
                    break;
                    }
            return View(modelToSort.ToList());
        }
        
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Create()
        {
            return RedirectToAction("Create", "Cars");
        }

        public async Task<IActionResult> Details(int? id)
        {
            return RedirectToAction("Details", "Cars", new { id = id });
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            return RedirectToAction("Edit", "Cars", new { id = id });
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            return RedirectToAction("Delete", "Cars", new { id = id });
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
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