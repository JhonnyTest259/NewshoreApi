using Newshore.Models;

namespace Newshore.Repository
{
    public class FlightRepository : IFlightRepository
    {
        public List<Flights> GetFlights(string origin, string destination, IEnumerable<Flight> externalApiData)
        {
            var flightDataTmp = externalApiData
                .Where(f => f.DepartureStation == origin)
                .ToList();

            var flights = new List<Flights>();

            foreach (var datatmp in flightDataTmp)
            {
                if (datatmp.DepartureStation == origin && datatmp.ArrivalStation == destination)
                {
                    flights.Add(new Flights
                    {
                        Origin = datatmp.DepartureStation,
                        Destination = datatmp.ArrivalStation,
                        Transport = new Transport
                        {
                            FlightCarrier = datatmp.FlightCarrier,
                            FlightNumber = datatmp.FlightNumber,
                        },
                        Price = datatmp.Price,
                    });
                }
            }

            if (flights.Count == 0)
            {
                var ruta = new List<Flights>();
                var vuelosPorSalida = new Dictionary<string, List<Flight>>();
                var vuelosPorLlegada = new Dictionary<string, List<Flight>>();

                foreach (var vuelo in externalApiData)
                {
                    if (!vuelosPorSalida.ContainsKey(vuelo.DepartureStation))
                        vuelosPorSalida[vuelo.DepartureStation] = new List<Flight>();

                    vuelosPorSalida[vuelo.DepartureStation].Add(vuelo);

                    if (!vuelosPorLlegada.ContainsKey(vuelo.ArrivalStation))
                        vuelosPorLlegada[vuelo.ArrivalStation] = new List<Flight>();

                    vuelosPorLlegada[vuelo.ArrivalStation].Add(vuelo);
                }

                var visitados = new HashSet<string>();
                var cola = new Queue<(Flight vuelo, Nodo padre)>();

                foreach (var vuelo in vuelosPorSalida.GetValueOrDefault(origin) ?? Enumerable.Empty<Flight>())
                {
                    cola.Enqueue((vuelo, null));
                }

                while (cola.Count > 0)
                {
                    var (vueloActual, nodoPadre) = cola.Dequeue();
                    if (vueloActual.ArrivalStation == destination)
                    {
                        ruta = new List<Flights>();
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
                    }
                    else
                    {
                        visitados.Add(vueloActual.DepartureStation);
                        var vuelosSiguientes = vuelosPorSalida.GetValueOrDefault(vueloActual.ArrivalStation) ?? Enumerable.Empty<Flight>();

                        foreach (var vueloSiguiente in vuelosSiguientes)
                        {
                            if (!visitados.Contains(vueloSiguiente.DepartureStation))
                            {
                                cola.Enqueue((vueloSiguiente, new Nodo(vueloActual, nodoPadre)));
                            }
                        }
                    }
                }

                flights.AddRange(ruta.Select(element => new Flights
                {
                    Origin = element.Origin,
                    Destination = element.Destination,
                    Transport = new Transport
                    {
                        FlightCarrier = element.Transport.FlightCarrier,
                        FlightNumber = element.Transport.FlightNumber,
                    },
                    Price = element.Price,
                }));
            }

            return flights;
        }
    }
}
