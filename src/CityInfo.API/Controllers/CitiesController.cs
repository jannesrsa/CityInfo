using System.Collections.Generic;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    //[Route("api/[controller]")]
    public class CitiesController : Controller
    {
        private readonly ICityInfoRepository _repository;

        public CitiesController(ICityInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cityEntities = _repository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _repository.GetCity(id, includePointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }

            if (!includePointsOfInterest)
            {
                return Ok(Mapper.Map<City, CityWithoutPointsOfInterestDto>(city));
            }
            else
            {
                return Ok(Mapper.Map<City, CityDto>(city));
            }
        }
    }
}