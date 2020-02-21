using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.DTOs;
using ParkyApi.Repository.IRepository;

namespace ParkyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkObj = _nationalParkRepository.GetNationalParks();
            var nationalParkObjDto = new List<NationalParkDto>();
            foreach (var obj in nationalParkObj)
            {
                nationalParkObjDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(nationalParkObjDto);
        }

        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalParkobj = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalParkobj == null)
            {
                return NotFound();
            }
            var nationalParkObjDto = _mapper.Map<NationalParkDto>(nationalParkobj);
            return Ok(nationalParkObjDto);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError(String.Empty, $"National park {nationalParkDto.Name} already exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_nationalParkRepository.CreateNationalPark(_mapper.Map<NationalPark>(nationalParkDto)))
            {
                ModelState.AddModelError(String.Empty, $"Something went wrong while trying to save {nationalParkDto.Name}");
                return StatusCode(401, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkDto.Id }, nationalParkDto);
        }

        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_nationalParkRepository.UpdateNationalPark(_mapper.Map<NationalPark>(nationalParkDto)))
            {
                ModelState.AddModelError(String.Empty, $"Something went wrong while updating the record {nationalParkDto.Name}");
                return StatusCode(401, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }
            var nationalParkObj = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (!_nationalParkRepository.DeleteNationalPark(_mapper.Map<NationalPark>(nationalParkObj)))
            {
                ModelState.AddModelError(String.Empty, $"Something went wrong while Deleting the record {nationalParkObj.Name}");
                return StatusCode(401, ModelState);
            }
            return NoContent();
        }



    }
}