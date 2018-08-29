using System.Linq;
using CityInfo.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("test/demo")]
    public class DemoController : Controller
    {
        private readonly CityInfoContext _context;

        public DemoController(CityInfoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok();
        }

    }
}