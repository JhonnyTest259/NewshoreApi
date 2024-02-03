using Newshore.Models;

namespace Newshore.Services
{
    public interface IFlightService
    {
        public Task<IEnumerable<Flight>> Get();
    }
}
