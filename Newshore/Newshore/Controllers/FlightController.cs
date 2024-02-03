
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
        public async Task<ActionResult<Journey>> GetJourney([FromQuery] string origin, [FromQuery] string destination)
                => await _flightsService.GetJourney(origin, destination);
    }
}
