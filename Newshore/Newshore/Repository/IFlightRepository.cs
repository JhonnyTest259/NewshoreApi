using Newshore.Models;

namespace Newshore.Repository
{
    public interface IFlightRepository
    {
        List<Flights> GetFlights(string origin, string destination, IEnumerable<Flight> externalApiData);
    }
}
