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
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkyOpenApiSpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParkV2Controller : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParkV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
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
            var obj = _nationalParkRepository.GetNationalParks().FirstOrDefault();
            //var nationalParkObjDto = new List<NationalParkDto>();
            //foreach (var obj in nationalParkObj)
            //{
            //    nationalParkObjDto.Add(_mapper.Map<NationalParkDto>(obj));
            //}
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }

    }
}