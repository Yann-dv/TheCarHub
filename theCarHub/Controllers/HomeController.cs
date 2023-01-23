﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using theCarHub.Models;

namespace theCarHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       // private readonly ApiController _apiController;

        public HomeController(ILogger<HomeController> logger)//, ApiController apiController)
        {
            _logger = logger;
            //_apiController = apiController;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}