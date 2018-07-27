using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class PointsOfInterestController : Controller
    {
        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult Get(int cityId)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(i => i.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}")]
        public IActionResult Get(int cityid, int id)
        {
            var pointOfInterest = CitiesDataStore.Current.Cities
               .FirstOrDefault(i => i.Id == cityid)
               ?.PointsOfInterest.FirstOrDefault(i => i.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        // POST: api/PointsOfInterest
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/PointsOfInterest/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
