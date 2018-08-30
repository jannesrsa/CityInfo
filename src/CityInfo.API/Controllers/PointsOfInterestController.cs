﻿using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class PointsOfInterestController : Controller
    {
        private const string GetName = "GetPointOfInterest";
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository repository)
        {
            _logger = logger;
            _mailService = mailService;
            _repository = repository;
            //HttpContext.RequestServices.GetService(typeof(ILogger<PointsOfInterestController>));
        }

        private readonly ICityInfoRepository _repository;

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "Description and name should be different");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxpointofinterestid = CitiesDataStore.Current.Cities
                .SelectMany(c => c.PointsOfInterest)
                .Max(p => p.Id) + 1;

            var finalPointOfInterest = new PointOfInterestDto
            {
                Id = maxpointofinterestid,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute(GetName,
                new { cityId, finalPointOfInterest.Id },
                finalPointOfInterest);
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult Delete(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var currentPointOfInterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            if (currentPointOfInterest == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(currentPointOfInterest);
            _mailService.Send("Deleted POI", $"Deleted POI is {currentPointOfInterest.Name}");
            _logger.LogCritical($"Deleted POI is {currentPointOfInterest.Name}");
            return NoContent();
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetsPointOfInterests(int cityId)
        {
            if (!_repository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't foun when access POI");
                return NotFound();
            }

            var results = new List<PointOfInterestDto>();

            var pois = _repository.GetPointsOfInterestForCity(cityId)
                .ToList();

            pois.ForEach(i => results.Add(PointOfInterestDto.CreateFromPointOfInterest(i)));

            return Ok(results);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = GetName)]
        public IActionResult GetsPointOfInterest(int cityId, int id)
        {
            if (!_repository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't foun when access POI");
                return NotFound();
            }

            var pointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(PointOfInterestDto.CreateFromPointOfInterest(pointOfInterest));
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartialUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var currentPointOfInterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            if (currentPointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto
            {
                Name = currentPointOfInterest.Name,
                Description = currentPointOfInterest.Description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            currentPointOfInterest.Name = pointOfInterestToPatch.Name;
            currentPointOfInterest.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "Description and name should be different");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var currentPointOfInterest = city.PointsOfInterest.FirstOrDefault(c => c.Id == id);
            if (currentPointOfInterest == null)
            {
                return NotFound();
            }

            currentPointOfInterest.Name = pointOfInterest.Name;
            currentPointOfInterest.Description = pointOfInterest.Description;

            return NoContent();
        }
    }
}