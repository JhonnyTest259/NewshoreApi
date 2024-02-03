using Newshore.Models;

namespace Newshore.Repository
{
    public interface IFlightRepository
    {
        List<Flights> GetFlights(string origin, string destination, List<Flight> externalApiData);
    }
}
