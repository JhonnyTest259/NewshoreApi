using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Newshore.Models
{
    public class Nodo
    {
        public Flight Vuelo { get; }
        public Nodo Padre { get; }

        public Nodo(Flight vuelo, Nodo padre)
        {
            Vuelo = vuelo;
            Padre = padre;
        }
        public List<Flight> ObtenerRuta()
        {
            var ruta = new List<Flight>();
            var nodo = this;
            while (nodo != null)
            {
                ruta.Insert(0, nodo.Vuelo);
                nodo = nodo.Padre;
            }
            return ruta;
        }
    }
}
