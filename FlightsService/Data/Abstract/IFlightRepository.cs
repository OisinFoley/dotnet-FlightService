using FlightsService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightsService.Data.Abstract
{
    public interface IFlightRepository : IBaseRepository<Flight>
    {
        IEnumerable<Flight> Flights { get; }

        Task<List<Flight>> GetFlights();
    }
}
