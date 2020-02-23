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
    [ApiExplorerSettings(GroupName = "ParkyOpenApiSpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParkController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// /Get all National parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] 
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

        /// <summary>
        /// Get an individual national park
        /// </summary>
        /// <param name="nationalParkId"> The Id of the National Park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        /// <summary>
        /// Create a new national park record
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkDto.Id }, nationalParkDto);
        }


        /// <summary>
        /// Update an existing national park record
        /// </summary>
        /// <param name="nationalParkId"> The record Id to be updated</param>
        /// <param name="nationalParkDto"> The body parameters</param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if(nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_nationalParkRepository.UpdateNationalPark(_mapper.Map<NationalPark>(nationalParkDto)))
            {
                ModelState.AddModelError(String.Empty, $"Something went wrong while updating the record {nationalParkDto.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Delete an existing national park record
        /// </summary>
        /// <param name="nationalParkId"> The Id of the record to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }



    }
}