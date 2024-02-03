
using Newshore.Models;
using System.Text.Json;

namespace Newshore.Services
{

    public class FlightService : IFlightService
    {
        private HttpClient _httpClient;

        public FlightService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<Flight>> Get()
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
    }
}
