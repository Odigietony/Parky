using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            foreach(var obj in nationalParkObj)
            {
                nationalParkObjDto.Add(_mapper.Map<NationalParkDto>(obj));
            }
            return Ok(nationalParkObjDto);
        }

        [HttpGet("{nationalParkId:int}")]
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
    }
}