using Newshore.Models;

namespace Newshore.Services
{
    public interface IFlightService
    {

        public List<string> Errors { get; }
        public Task<IEnumerable<Flight>> GetExternalApiData();
        public Task<Journey> GetJourney(string origin, string destination, int? flyLimit);
        bool ValidateLenghtOfFlights(List<Flights> flights, int? flyLimit);
    }
}
