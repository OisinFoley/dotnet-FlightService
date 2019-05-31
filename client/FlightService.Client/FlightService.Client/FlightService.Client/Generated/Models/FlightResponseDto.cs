// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace FlightService.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class FlightResponseDto
    {
        /// <summary>
        /// Initializes a new instance of the FlightResponseDto class.
        /// </summary>
        public FlightResponseDto()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the FlightResponseDto class.
        /// </summary>
        public FlightResponseDto(System.Guid? id = default(System.Guid?), string departureLocation = default(string), string arrivalLocation = default(string), string date = default(string), int? basePrice = default(int?), int? availableSeats = default(int?), bool? isSpecialOffer = default(bool?))
        {
            Id = id;
            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;
            Date = date;
            BasePrice = basePrice;
            AvailableSeats = availableSeats;
            IsSpecialOffer = isSpecialOffer;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public System.Guid? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "departureLocation")]
        public string DepartureLocation { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "arrivalLocation")]
        public string ArrivalLocation { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "basePrice")]
        public int? BasePrice { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "availableSeats")]
        public int? AvailableSeats { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isSpecialOffer")]
        public bool? IsSpecialOffer { get; set; }

    }
}
