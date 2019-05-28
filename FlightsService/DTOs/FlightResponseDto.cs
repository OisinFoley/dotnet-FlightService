using System;
namespace FlightsService.DTOs
{
    public class FlightResponseDto: FlightRequestDto
    {
        public Guid Id { get; set; }
    }
}
