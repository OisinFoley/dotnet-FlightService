using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace FlightsService.DTOs
{
    public class FlightDto
    {
        public Guid Id { get; set; }
        [JsonProperty(PropertyName = "departureLocation")]
        public string DepartureLocation { get; set; }
        [JsonProperty(PropertyName = "arrivalLocation")]
        public string ArrivalLocation { get; set; }
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }
        [JsonProperty(PropertyName = "basePrice")]
        public int BasePrice { get; set; }
        [JsonProperty(PropertyName = "availableSeats")]
        public int AvailableSeats { get; set; }
        [JsonProperty(PropertyName = "isSpecialOffer")]
        public bool IsSpecialOffer { get; set; }
    }
}
