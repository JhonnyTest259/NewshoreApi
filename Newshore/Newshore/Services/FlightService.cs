
using Newshore.Models;
using Newshore.Repository;
using System.Text.Json;

namespace Newshore.Services
{

    public class FlightService : IFlightService
    {
        private HttpClient _httpClient;
        private IFlightRepository _flightRepository;

        public FlightService(HttpClient httpClient, 
            IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
            _httpClient = httpClient;
        }

        #region ApiCall
        public async Task<IEnumerable<Flight>> GetExternalApiData()
        {
            var result = await _httpClient.GetAsync(_httpClient.BaseAddress);
            var body = await result.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var flight = JsonSerializer.Deserialize<IEnumerable<Flight>>(body, options);

            return flight;
        }
        #endregion


        public async Task<Journey> GetJourney(string origin, string destination)
        {
            var externalApiData = await GetExternalApiData();

            var flights = _flightRepository.GetFlights(origin, destination, externalApiData);

            var journey = new Journey
            {
                Flights = [],
                Origin = origin,
                Destination = destination,
                Price = 300f
            };

            return journey;
        }
    }
}
