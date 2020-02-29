using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if(ModelState.IsValid)
            {
                /*
                  An image file can be uploaded both during the create new or edit record.
                  from the incoming forms object, get the file convert from a stream to memory stream of array and 
                  parse it to the national park picture object as a byte.
                 */
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    byte[] picture1 = null;
                    using(var file = files[0].OpenReadStream())
                    {
                        using(var memoryStream = new MemoryStream())
                        {
                            file.CopyTo(memoryStream);
                            picture1 = memoryStream.ToArray();
                        }
                    }
                    nationalPark.Picture = picture1;
                }
                else
                {
                    var nationalParkObjectformDb = await _nationalPark.GetAsync(StaticDetails.NationalParkApiUrl, nationalPark.Id);
                    nationalPark.Picture = nationalParkObjectformDb.Picture;
                }
                if(nationalPark.Id == 0)
                {
                    await _nationalPark.CreateAsync(StaticDetails.NationalParkApiUrl, nationalPark);
                }
                else
                {
                    await _nationalPark.UpdateAsync(StaticDetails.NationalParkApiUrl + nationalPark.Id, nationalPark);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nationalPark);
            }
        }

        public async Task<IActionResult> GetAllNationalParks()
        {
            return Json(new { data = await _nationalPark.GetAllAsync(StaticDetails.NationalParkApiUrl)});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var nationalParkObject = await _nationalPark.GetAsync(StaticDetails.NationalParkApiUrl, Id);
            var status = await _nationalPark.DeleteAsync(StaticDetails.NationalParkApiUrl, Id);
            if(status)
            {
                return Json(new { success = true, Message = $"The {nationalParkObject.Name} was Deleted Successfully!" });
            }
            return Json(new { success = false, Message = $"Delete was not Successful!" });
        }
    }
}