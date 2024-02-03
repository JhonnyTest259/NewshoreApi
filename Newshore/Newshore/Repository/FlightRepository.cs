using Newshore.Models;

namespace Newshore.Repository
{
    public class FlightRepository : IFlightRepository
    {
        public List<Flights> GetFlights(string origin, string destination, List<Flight> externalApiData)
        {
            var flights = new List<Flights>
            {
            
            };

            return flights;
        }
    }
}
