using System.Collections.Generic;
namespace FlightsService.DTOs
{
    public class FlightsResponseDto
    {
        public IEnumerable<FlightResponseDto> Flights { get; set; }
    }
}
