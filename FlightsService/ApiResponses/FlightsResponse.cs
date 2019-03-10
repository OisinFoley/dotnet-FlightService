using FlightsService.DTOs;
using System.Collections.Generic;

namespace FlightsService.ApiResponses
{
    public sealed class FlightsResponse
    {
        public IEnumerable<FlightDto> Flights { get; set; }
    }
}
