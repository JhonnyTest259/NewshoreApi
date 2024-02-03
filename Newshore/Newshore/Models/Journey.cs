
namespace Newshore.Models
{
    public class Journey
    {
        public List<Flights> Flights { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }
    }
}
