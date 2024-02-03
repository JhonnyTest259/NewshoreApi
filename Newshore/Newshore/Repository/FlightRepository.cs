using Newshore.Models;

namespace Newshore.Repository
{
    public class FlightRepository : IFlightRepository
    {
        #region GetFlights
        public List<Flights> GetFlights(string origin, string destination, IEnumerable<Flight> externalApiData)
        {
            var directFlights = GetDirectFlights(origin, destination, externalApiData);
            if (directFlights.Count > 0)
            {
                return directFlights;
            }

            var flightsByDeparture = GroupFlightsByDeparture(externalApiData);
            var ruta = GetRoutes(origin, destination, flightsByDeparture);

            return ruta;
        }
        #endregion

        #region GetDirectFlights
        public List<Flights> GetDirectFlights(string origin, string destination, IEnumerable<Flight> externalApiData)
        {
            return externalApiData
                .Where(f => f.DepartureStation == origin && f.ArrivalStation == destination)
                .Select(f => new Flights
                {
                    Origin = f.DepartureStation,
                    Destination = f.ArrivalStation,
                    Transport = new Transport
                    {
                        FlightCarrier = f.FlightCarrier,
                        FlightNumber = f.FlightNumber,
                    },
                    Price = f.Price,
                })
                .ToList();
        }
        #endregion


        #region GroupFlightsByDeparture
        public Dictionary<string, List<Flight>> GroupFlightsByDeparture(IEnumerable<Flight> externalApiData)
        {
            return externalApiData
                .GroupBy(f => f.DepartureStation)
                .ToDictionary(group => group.Key, group => group.ToList());
        }
        #endregion

        #region GetRoutes
        public List<Flights> GetRoutes(string origin, string destination, Dictionary<string, List<Flight>> flightsByDeparture)
        {
            var visitados = new HashSet<string>();
            var cola = new Queue<(Flight vuelo, Nodo padre)>();
            var ruta = new List<Flights>();

            foreach (var vuelo in flightsByDeparture.GetValueOrDefault(origin) ?? Enumerable.Empty<Flight>())
            {
                cola.Enqueue((vuelo, null));
            }

            while (cola.Count > 0)
            {
                var (vueloActual, nodoPadre) = cola.Dequeue();
                if (vueloActual.ArrivalStation == destination)
                {
                    var nodo = new Nodo(vueloActual, nodoPadre);

                    while (nodo != null)
                    {
                        ruta.Insert(0, new Flights
                        {
                            Origin = nodo.Vuelo.DepartureStation,
                            Destination = nodo.Vuelo.ArrivalStation,
                            Transport = new Transport
                            {
                                FlightCarrier = nodo.Vuelo.FlightCarrier,
                                FlightNumber = nodo.Vuelo.FlightNumber,
                            },
                            Price = nodo.Vuelo.Price,
                        });

                        nodo = nodo.Padre;
                    }

                    break;
                }
                else
                {
                    visitados.Add(vueloActual.DepartureStation);
                    var vuelosSiguientes = flightsByDeparture.GetValueOrDefault(vueloActual.ArrivalStation) ?? Enumerable.Empty<Flight>();

                    foreach (var vueloSiguiente in vuelosSiguientes)
                    {
                        if (!visitados.Contains(vueloSiguiente.DepartureStation))
                        {
                            cola.Enqueue((vueloSiguiente, new Nodo(vueloActual, nodoPadre)));
                        }
                    }
                }
            }

            return ruta;
        }
        #endregion
    }
}
