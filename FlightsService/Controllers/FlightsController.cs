using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightsService.Extensions;
using FlightsService.Models;
using FlightsService.ApiRequests;
using FlightsService.Data.Abstract;
using Microsoft.AspNetCore.Http;
using FlightsService.ApiResponses;
using FlightsService.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FlightsService.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightRepository m_FlightRepository;

        public FlightsController(IFlightRepository flightRepository)
        {
            m_FlightRepository = flightRepository;
        }

        // GET api/v1/flights
        [HttpGet("api/v1/flights")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<OkObjectResult> Get(string departureLocation, 
            string arrivalLocation, string nothing, string date, string SOMETHINGELSE)
        {
            IEnumerable<Flight> flights = await m_FlightRepository.GetFlights();

            flights = flights.Where(f => f.DepartureLocation.Contains(departureLocation));

            return new OkObjectResult(flights);
        }        

        // POST api/v1/flights
        [HttpPost("api/v1/flights")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<FlightResponse> Post([FromBody] FlightRequest flightRequest)
        {
            Flight insertedFlight = await m_FlightRepository.InsertAsync(await GetHydratedLicenseBindingAsync(flightRequest.Flight));
            
            var response = new FlightResponse { Flight = insertedFlight.ToFlightDto() };
            return response;
        }

        private async Task<Flight> GetHydratedLicenseBindingAsync(FlightDto dto)
        {
            Flight flight = await m_FlightRepository.FindAsync(dto.Id);
            return dto.ToFlight(flight);
        }
    }
}
