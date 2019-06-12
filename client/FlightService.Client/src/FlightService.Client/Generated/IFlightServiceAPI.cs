// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace FlightService.Client
{
    using Microsoft.Rest;
    using Models;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Simple API to perform CRUDs on Flights
    /// </summary>
    public partial interface IFlightServiceAPI : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }


        /// <summary>
        /// Retrieves all Flights matching the provided property parameters.
        /// </summary>
        /// <param name='departureLocation'>
        /// </param>
        /// <param name='arrivalLocation'>
        /// </param>
        /// <param name='date'>
        /// </param>
        /// <param name='basePrice'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<FlightsResponse>> GetWithHttpMessagesAsync(string departureLocation = default(string), string arrivalLocation = default(string), string date = default(string), int? basePrice = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds a new Flight.
        /// </summary>
        /// <param name='flightRequest'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<FlightResponse>> PostWithHttpMessagesAsync(FlightRequest flightRequest = default(FlightRequest), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Retrieves a specific Flight.
        /// </summary>
        /// <param name='id'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<FlightResponse>> Get1WithHttpMessagesAsync(System.Guid id, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates a specific existing Flight.
        /// </summary>
        /// <param name='id'>
        /// </param>
        /// <param name='flightRequest'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<FlightResponse>> PutWithHttpMessagesAsync(System.Guid id, FlightRequest flightRequest = default(FlightRequest), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes a specific Flight.
        /// </summary>
        /// <param name='id'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse> DeleteWithHttpMessagesAsync(System.Guid id, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));

    }
}
