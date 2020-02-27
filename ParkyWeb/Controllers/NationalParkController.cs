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
            return View();
        }

        public async Task<IActionResult> Upsert(int? Id)
        {
            NationalPark nationalPark = new NationalPark();
            if(Id == null)
            {
                return View(nationalPark);
            }
            nationalPark = await _nationalPark.GetAsync(StaticDetails.NationalParkApiUrl, Id.GetValueOrDefault());
            if(nationalPark == null)
            {
                return NotFound();
            }
            return View(nationalPark);
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new { data = await _nationalPark.GetAllAsync(StaticDetails.NationalParkApiUrl)});
        }
    }
}