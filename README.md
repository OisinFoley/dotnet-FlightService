# Flight REST API Service

## Dotnet Core Web API Application for adding flights to a SQL Azure database

## Features

- POST requests to add new flights
- Use of Azure Service Bus to subscribe to messages regarding the addition of bookings for a flight, which take place in a separate API
(https://github.com/OisinFoley/dotnet-BookingService/blob/master/README.md)
- Repository Pattern, Unit of Work Pattern
- Singleton Pattern for IncomingMessage service which forms Service Bus to Azure
- Use of DTOs to carry data between layers

## Requirements

- Add your appsettings for 

``` 
"DatabaseHost": "SQLServer",
"ConnectionStrings": {
    "SQLServer": "<something-here>" 
  },
  "MessageServiceCommon": { 
    "ConnectionString": "<something-here>", //service-bus connection string 
    "TopicName": "<something-here>",
}
"InboundMessageService": {
    "PollingIntervalInMilliseconds": <something-here>,
    "SubscriptionName": "<something-here>"
} 
```

- Dotnet Core 2.1 [Link to microsoft downloads website](https://dotnet.microsoft.com/download)
- .NET SDK [Link to microsoft downloads website](https://dotnet.microsoft.com/download)
- Download the EventDispatcher library from this account too
- Add a reference to the EventDispatcher Nuget once downloaded