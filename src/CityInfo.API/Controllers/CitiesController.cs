using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    //[Route("api/[controller]")]
    public class CitiesController : Controller
    {
        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(i => i.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }
    }
}