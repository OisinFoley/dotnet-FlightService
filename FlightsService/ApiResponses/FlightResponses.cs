using FlightsService.DTOs;
using System.Collections.Generic;

namespace FlightsService.ApiResponses
{
    public sealed class FlightResponses
    {
        public IEnumerable<FlightDto> Flights { get; }
    }
}
