
using Newshore.Models;
using Newshore.Repository;
using System.Text.Json;

namespace Newshore.Services
{

    public class FlightService : IFlightService
    {

        public List<string> Errors { get; }

        private HttpClient _httpClient;
        private IFlightRepository _flightRepository;

        public FlightService(HttpClient httpClient, 
            IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
            _httpClient = httpClient;
            Errors = new List<string>();
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


        public async Task<Journey> GetJourney(string origin, string destination, int? flyLimit)
        {
            var externalApiData = await GetExternalApiData();

            var flights = _flightRepository.GetFlights(origin, destination, externalApiData);

            if(ValidateLenghtOfFlights(flights,flyLimit))
            {
                var journey = new Journey
                {
                    Flights = flights,
                    Origin = origin,
                    Destination = destination,
                    Price = flights.Sum(f => f.Price)
                };

                return journey;
            }

            return null;
           
        }

        public bool ValidateLenghtOfFlights(List<Flights> flights, int? flyLimit = 0)
        {
            if(flights.Count == 0)
            {
                Errors.Add("La consulta no puede ser procesada.");
                return false;
            }

            if(flyLimit != 0 && flights.Count > flyLimit)
            {
                Errors.Add("Se supero el numero de viajes.");
                return false;
            }
            return true;
        }
    }
}
