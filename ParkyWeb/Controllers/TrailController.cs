using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.ApiEndpoints;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class TrailController : Controller
    {
        private readonly INationalParkRepository _nationalPark;
        private readonly ITrailRepository _trailRepository;

        public TrailController(INationalParkRepository nationalPark, ITrailRepository trailRepository)
        {
            _nationalPark = nationalPark;
            _trailRepository = trailRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? Id)
        { 
            var nationalParksList = await _nationalPark.GetAllAsync(StaticDetails.NationalParkApiUrl);
            TrailViewModel trailViewModel = new TrailViewModel()
            {
                NationalParks = nationalParksList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (Id == null)
            {
                return View(trailViewModel);
            }
            trailViewModel.Trail = await _trailRepository.GetAsync(StaticDetails.TrailsApiUrl, Id.GetValueOrDefault());
            if (trailViewModel.Trail == null)
            {
                return NotFound();
            }
            return View(trailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailViewModel model)
        {
            if (ModelState.IsValid)
            { 
                if (model.Trail.Id == 0)
                {
                    await _trailRepository.CreateAsync(StaticDetails.TrailsApiUrl, model.Trail);
                }
                else
                {
                    await _trailRepository.UpdateAsync(StaticDetails.TrailsApiUrl + model.Trail.Id, model.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(StaticDetails.TrailsApiUrl) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var trailsObj = await _trailRepository.GetAsync(StaticDetails.TrailsApiUrl, Id);
            var status = await _trailRepository.DeleteAsync(StaticDetails.TrailsApiUrl, Id);
            if (status)
            {
                return Json(new { success = true, Message = $"The {trailsObj.Name} was Deleted Successfully!" });
            }
            return Json(new { success = false, Message = $"Delete was not Successful!" });
        }
    }
}