using FlightsService.DTOs;
using FlightsService.Models;

namespace FlightsService.Extensions
{
    public static class ConverterExtension
    {
        public static FlightDto ToFlightDto(this Flight flight) => new FlightDto
        {
            Id = flight.Id,
            DepartureLocation = flight.DepartureLocation,
            ArrivalLocation = flight.ArrivalLocation,
            Date = flight.Date,
            BasePrice = flight.BasePrice,
            AvailableSeats = flight.AvailableSeats,
            IsSpecialOffer = flight.IsSpecialOffer
        };

        public static Flight ToFlight(
            this FlightDto dto,
            Flight existingFlight = null)
        {

            if (existingFlight == null)
                return new Flight
                {
                    Id = dto.Id,
                    DepartureLocation = dto.DepartureLocation,
                    ArrivalLocation = dto.ArrivalLocation,
                    Date = dto.Date,
                    BasePrice = dto.BasePrice,
                    AvailableSeats = dto.AvailableSeats,
                    IsSpecialOffer = dto.IsSpecialOffer
                };

            existingFlight.DepartureLocation = dto.DepartureLocation;
            existingFlight.ArrivalLocation = dto.ArrivalLocation;
            existingFlight.Date = dto.Date;
            existingFlight.BasePrice = dto.BasePrice;
            existingFlight.AvailableSeats = dto.AvailableSeats;
            existingFlight.IsSpecialOffer = dto.IsSpecialOffer;

            return existingFlight;
        }
    }
}
