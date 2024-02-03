
using Microsoft.AspNetCore.Mvc;
using Newshore.Models;
using Newshore.Services;

namespace Newshore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {

        IFlightService _flightsService;
        public FlightController(IFlightService flightsService)
        {
            _flightsService = flightsService;
        }

        [HttpGet("search")]
        public ActionResult<string> GetJourney([FromQuery] string origin, [FromQuery] string destination)
        {
            return Ok($"Origin: {origin}, Destination: {destination}");
        }
    }
}
