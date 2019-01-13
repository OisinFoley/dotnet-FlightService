using FlightsService.Data.Abstract;
using FlightsService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightsService.Data.Concrete
{
    public sealed class FlightRepository : BaseRepository<Flight>, IFlightRepository
    {
        public FlightRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
        }

        IEnumerable<Flight> IFlightRepository.Flights => GetDbSet().AsEnumerable();

        public async Task<List<Flight>> GetFlights() =>
            await GetDbSet().ToListAsync();
    }
}
