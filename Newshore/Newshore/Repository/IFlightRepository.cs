using Newshore.Models;

namespace Newshore.Repository
{
    public interface IFlightRepository
    {
        List<Flights> GetFlights(string origin, string destination, IEnumerable<Flight> externalApiData);
        List<Flights> GetDirectFlights(string origin, string destination, IEnumerable<Flight> externalApiData);
        Dictionary<string, List<Flight>> GroupFlightsByDeparture(IEnumerable<Flight> externalApiData);
        List<Flights> GetRoutes(string origin, string destination, Dictionary<string, List<Flight>> flightsByDeparture);
    }
}
