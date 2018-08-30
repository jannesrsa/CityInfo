using System.Collections.Generic;
using AutoMapper;
using CityInfo.API.Entities;
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

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = Mapper.Map<PointOfInterestForCreationDto, PointOfInterest>(pointOfInterest);
            _repository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if (!_repository.Save())
            {
                return StatusCode(500, "Error");
            }

            var dto = Mapper.Map<PointOfInterest, PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute(GetName,
                new { cityId, finalPointOfInterest.Id },
                dto);
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult Delete(int cityId, int id)
        {
            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var currentPointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);
            if (currentPointOfInterest == null)
            {
                return NotFound();
            }

            _repository.DeletePointOfInterest(currentPointOfInterest);

            if (!_repository.Save())
            {
                return StatusCode(500, "Error");
            }

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

            var pointsOfInterest = _repository.GetPointsOfInterestForCity(cityId);
            return Ok(Mapper.Map<IEnumerable<PointOfInterest>, IEnumerable<PointOfInterestDto>>(pointsOfInterest));
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

            return Ok(Mapper.Map<PointOfInterest, PointOfInterestDto>(pointOfInterest));
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartialUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_repository.CityExists(cityId))
            {
                return NotFound();
            }

            var currentPointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);
            if (currentPointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(currentPointOfInterest);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Mapper.Map(pointOfInterestToPatch, currentPointOfInterest);

            if (!_repository.Save())
            {
                return StatusCode(500);
            }

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

            if (!_repository.CityExists(cityId))
            {
                return NotFound("City");
            }

            var currentPointOfInterest = _repository.GetPointOfInterestForCity(cityId, id);
            if (currentPointOfInterest == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, currentPointOfInterest);

            if (!_repository.Save())
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}