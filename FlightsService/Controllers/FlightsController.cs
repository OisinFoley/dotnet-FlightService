﻿using System.Collections.Generic;
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
    [ApiController]
    public class FlightsController : Controller
    {
        private readonly IFlightRepository m_FlightRepository;

        public FlightsController(IFlightRepository flightRepository)
        {
            m_FlightRepository = flightRepository;
        }

        /// <summary>
        /// Retrieves all Flights matching the provided property parameters.
        /// </summary>
        [HttpGet("api/v1/flights")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FlightsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string departureLocation, 
            string arrivalLocation, 
            string date, 
            int basePrice)
        {
            IEnumerable<Flight> flights = await m_FlightRepository.GetFlights();

            if (!string.IsNullOrEmpty(departureLocation))
                flights = flights
                    .Where(f => f.DepartureLocation.ToUpper().Contains(departureLocation.ToUpper()));
            if (!string.IsNullOrEmpty(arrivalLocation))
                flights = flights
                    .Where(f => f.ArrivalLocation.ToUpper().Contains(arrivalLocation.ToUpper()));
            if (!string.IsNullOrEmpty(date))
                flights = flights.Where(f => f.Date.Equals(date));
            if (basePrice > 0)
                flights = flights.Where(f => f.BasePrice <= (basePrice));

            var response = new FlightsResponse
            {
                Flights = flights.Select(flight => flight.ToFlightResponseDto())
            };

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a specific Flight.
        /// </summary>
        [HttpGet("api/v1/flights/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FlightResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new BadRequestResult();
            }

            Flight flight = await m_FlightRepository.FindAsync(id);
            if (flight == null)
                return NotFound();

            var response = new FlightResponse { Flight = flight.ToFlightResponseDto() };
            return Ok(response);
        }

        /// <summary>
        /// Adds a new Flight.
        /// </summary>
        [HttpPost("api/v1/flights")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FlightResponse), StatusCodes.Status200OK)]
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

            var response = new FlightResponse { Flight = insertedFlight.ToFlightResponseDto() };
            return Ok(response);
        }

        /// <summary>
        /// Updates a specific existing Flight.
        /// </summary>
        [HttpPut("api/v1/flights/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FlightResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] FlightRequest flightRequest)
        {
            if (flightRequest?.Flight == null)
            {
                return new BadRequestResult();
            }

            Flight flight = m_FlightRepository.Flights.SingleOrDefault(x => x.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            flight.ArrivalLocation = flightRequest.Flight.ArrivalLocation;
            flight.AvailableSeats = flightRequest.Flight.AvailableSeats;
            flight.BasePrice = flightRequest.Flight.BasePrice;
            flight.Date = flightRequest.Flight.Date;
            flight.DepartureLocation = flightRequest.Flight.DepartureLocation;
            flight.IsSpecialOffer = flightRequest.Flight.IsSpecialOffer;
            
            var flightResponse = await m_FlightRepository.UpdateAsync(flight);
            var response = new FlightResponse { Flight = flight.ToFlightResponseDto() };

            return Ok(response);
        }

        /// <summary>
        /// Deletes a specific Flight.
        /// </summary>
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
    }
}
