using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.ApiEndpoints;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalPark;

        public NationalParkController(INationalParkRepository nationalPark)
        {
            _nationalPark = nationalPark;
        }
        public IActionResult Index()
        {
            return View(new NationalPark { });
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new { data = await _nationalPark.GetAllAsync(StaticDetails.NationalParkApiUrl)});
        }
    }
}