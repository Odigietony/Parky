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
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenApiSpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// /Get all Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public IActionResult GetTrails()
        {
            var trailObj = _trailRepository.GetTrails();
            var trailObjDto = new List<TrailDto>();
            foreach (var obj in trailObj)
            {
                trailObjDto.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(trailObjDto);
        }

        /// <summary>
        /// Get an individual trails
        /// </summary>
        /// <param name="trailId"> The Id of the Trail </param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrail(int trailId)
        {
            var trailobj = _trailRepository.GetTrail(trailId);
            if (trailobj == null)
            {
                return NotFound();
            }
            var trailObjDto = _mapper.Map<TrailDto>(trailobj);
            return Ok(trailObjDto);
        }


        /// <summary>
        /// Get trail in national park
        /// </summary>
        /// <param name="nationalParkId"></param> 
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var trailListObj = _trailRepository.GetTrailsInNationalPark(nationalParkId);
            if (trailListObj == null)
            {
                return NotFound();
            }
            var trailObjDto = new List<TrailDto>();
            foreach (var trailobj in trailListObj)
            {
                 trailObjDto.Add(_mapper.Map<TrailDto>(trailobj));
            }
            
            return Ok(trailObjDto);
        }


        /// <summary>
        /// Create a new trail  record
        /// </summary>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError(String.Empty, $"Trail {trailDto.Name} already exists");
                return StatusCode(404, ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }


        /// <summary>
        /// Update an existing trail record
        /// </summary>
        /// <param name="trailId"> The record Id to be updated</param>
        /// <param name="trailDto"> The body parameters</param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if(trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            if (!_trailRepository.UpdateTrail(_mapper.Map<Trail>(trailDto)))
            {
                ModelState.AddModelError(String.Empty, $"Something went wrong while updating the record {trailDto.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Delete an existing trail record
        /// </summary>
        /// <param name="trailId"> The Id of the record to be deleted</param>
        /// <returns></returns>
        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }
            var trailObj = _trailRepository.GetTrail(trailId);
            if (!_trailRepository.DeleteTrail(_mapper.Map<Trail>(trailObj)))
            {
                ModelState.AddModelError(String.Empty, $"Something went wrong while Deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }



    }
}