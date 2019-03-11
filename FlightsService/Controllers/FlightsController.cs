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
using System;

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
        public async Task<IActionResult> Get(string departureLocation, 
            string arrivalLocation, 
            string date, 
            int basePrice)
        {
            IEnumerable<Flight> flights = await m_FlightRepository.GetFlights();

            if (!string.IsNullOrEmpty(departureLocation))
                flights = flights.Where(f => f.DepartureLocation.Contains(departureLocation));
            if (!string.IsNullOrEmpty(arrivalLocation))
                flights = flights.Where(f => f.ArrivalLocation.Contains(arrivalLocation));
            if (!string.IsNullOrEmpty(date))
                flights = flights.Where(f => f.Date.Equals(date));
            if (basePrice > 0)
                flights = flights.Where(f => f.BasePrice <= (basePrice));

            var response = new FlightsResponse
            {
                Flights = flights.Select(flight => flight.ToFlightDto())
            };

            return Ok(response);
        }

        // GET api/v1/flights/{flightId}
        [HttpGet("api/v1/flights/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new BadRequestResult();
            }

            Flight flight = await m_FlightRepository.FindAsync(id);
            if (flight == null)
                return NotFound();

            var response = new FlightResponse { Flight = flight.ToFlightDto() };
            return Ok(response);
        }

        // POST api/v1/flights
        [HttpPost("api/v1/flights")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] FlightRequest flightRequest)
        {
            if (flightRequest?.Flight == null)
            {
                return new BadRequestResult();
            }

            Flight newFlight = flightRequest.Flight.ToFlight();
            newFlight.Id = Guid.NewGuid();
            var insertedFlight = await m_FlightRepository.InsertAsync(newFlight);

            var response = new FlightResponse { Flight = insertedFlight.ToFlightDto() };
            return Ok(response);
        }

        // PUT: api/v1/flights/{flightId}
        [HttpPut("api/v1/flights/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] FlightRequest flightRequest)
        {
            if (flightRequest?.Flight == null)
            {
                return new BadRequestResult();
            }

            flightRequest.Flight.Id = id;

            if (!(m_FlightRepository.Flights.Any(x => x.Id == flightRequest.Flight.Id)))
            {
                return NotFound();
            }

            Flight flight = await GetHydratedFlightAsync(flightRequest.Flight);
            await m_FlightRepository.UpdateAsync(flight);

            var response = new FlightResponse { Flight = flight.ToFlightDto() };

            return Ok(response);
        }

        // DELETE: api/v1/flights/{flightId}
        [HttpDelete("api/v1/flights/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            Flight flight = m_FlightRepository.Flights.FirstOrDefault(b => b.Id == id);
            if (flight == null)
            {
                return new NotFoundResult();
            }

            await m_FlightRepository.DeleteAsync(flight);

            return Ok();
        }


        private async Task<Flight> GetHydratedFlightAsync(FlightDto dto)
        {
            Flight flight = await m_FlightRepository.FindAsync(dto.Id);
            return dto.ToFlight(flight);
        }
    }
}
