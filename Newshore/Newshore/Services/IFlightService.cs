using Newshore.Models;

namespace Newshore.Services
{
    public interface IFlightService
    {
        public Task<IEnumerable<Flight>> GetExternalApiData();

        public Task<Journey> GetJourney(string origin, string destination);
    }
}
