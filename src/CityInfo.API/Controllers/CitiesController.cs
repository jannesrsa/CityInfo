using System.Collections.Generic;
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
            var results = new List<CityWithoutPointsOfInterestDto>();

            foreach (var cityEntity in cityEntities)
            {
                results.Add(CityWithoutPointsOfInterestDto.CreateFromCity(cityEntity));
            }

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
                return Ok(CityWithoutPointsOfInterestDto.CreateFromCity(city));
            }
            else
            {
                return Ok(CityDto.CreateFromCity(city));
            }
        }
    }
}