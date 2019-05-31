using System;

namespace FlightService.Client
{
    public partial class FlightsServiceClient : FlightServiceAPI
    {
        public FlightsServiceClient()
        {
            BaseUri = new Uri("http://localhost:5000");
        }
    }
}
