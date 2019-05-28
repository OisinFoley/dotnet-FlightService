using FlightsService.ApiRequests;
using FlightsService.ApiResponses;
using FlightsService.Controllers;
using FlightsService.Data.Abstract;
using FlightsService.DTOs;
using FlightsService.Models;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlightsService.Tests
{
    
    public class FlightControllerTest
    {
        private readonly IFlightRepository m_FlightRepository;
        private readonly List<Flight> m_Flights;

        private const string Guid1 = "eb1e1206-d482-4b83-a613-a6db91ce732a";
        private const string DepartureLocation1 = "Dublin DUB";
        private const string ArrivalLocation1 = "Copenhagen CPH";
        private const string Date1 = "01/02/2023";
        private const int BasePrice1 = 100;
        private const int AvailableSeats1 = 300;
        private const bool IsSpecialOffer1 = false;

        private const string Guid2 = "ab3147bb-25d3-4c6b-bbdd-42d658b205c3";
        private const string DepartureLocation2 = "Knock KNO";
        private const string ArrivalLocation2 = "Stansted STN";
        private const string Date2 = "02/03/2024";
        private const int BasePrice2 = 200;
        private const int AvailableSeats2 = 200;
        private const bool IsSpecialOffer2 = false;

        private const string Guid3 = "b431a2ab-730c-4df0-8e9d-81fc74b5b4b8";
        private const string DepartureLocation3 = "Budapest BUD";
        private const string ArrivalLocation3 = "Sevilla SVQ";
        private const string Date3 = "03/04/2025";
        private const int BasePrice3 = 150;
        private const int AvailableSeats3 = 100;
        private const bool IsSpecialOffer3 = true;

        public FlightControllerTest()
        {
            m_Flights = new List<Flight>
            {
                new Flight { Id = new Guid(Guid1), DepartureLocation = DepartureLocation1, ArrivalLocation = ArrivalLocation1, Date = Date1,
                    BasePrice = BasePrice1, AvailableSeats = AvailableSeats1, IsSpecialOffer = IsSpecialOffer1 },
                new Flight { Id = new Guid(Guid2), DepartureLocation = DepartureLocation2, ArrivalLocation = ArrivalLocation2, Date = Date2,
                    BasePrice = BasePrice2, AvailableSeats = AvailableSeats2, IsSpecialOffer = IsSpecialOffer2 }
            };

            m_FlightRepository = Substitute.For<IFlightRepository>();

            m_FlightRepository.Flights.ReturnsForAnyArgs(m_Flights);
            m_FlightRepository.GetFlights()
                .Returns(m_Flights);

            m_FlightRepository.FindAsync(Arg.Any<object[]>())
                .Returns(x => m_FlightRepository.Flights.SingleOrDefault(flight => flight.Id == (Guid)x.Arg<object[]>()[0]));

            m_FlightRepository.InsertAsync(Arg.Any<Flight>())
                .Returns(x => x.Arg<Flight>())
                .AndDoes(y => m_Flights.Add(y.Arg<Flight>()));
            m_FlightRepository.UpdateAsync(Arg.Any<Flight>())
                .Returns(0)
                .AndDoes(y =>
                {
                    m_Flights.RemoveAll(w => w.Id == y.Arg<Flight>().Id);
                    m_Flights.Add(y.Arg<Flight>());
                });
            m_FlightRepository.DeleteAsync(Arg.Any<Flight>())
                .Returns(0)
                .AndDoes(y => m_Flights.Remove(y.Arg<Flight>()));
        }

        #region GetFlight(s)

        [Fact]
        public async Task GetFlightsReturnsOkObjectResponseAndAllFlightsWhenNoQueryStringArgumentsProvided()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);

            // Act
            IActionResult result = await flightController.Get(string.Empty, string.Empty, string.Empty, 0).ConfigureAwait(false);
            var parsedResult = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(parsedResult.StatusCode == 200);
            var response = Assert.IsType<FlightsResponse>((parsedResult.Value));

            Assert.Equal(2, response.Flights.Count());

            FlightResponseDto flight1 = response.Flights.Single(x => x.Id.ToString() == Guid1);
            Assert.Equal(DepartureLocation1, flight1.DepartureLocation);
            Assert.Equal(ArrivalLocation1, flight1.ArrivalLocation);
            Assert.Equal(IsSpecialOffer1, flight1.IsSpecialOffer);
            Assert.Equal(AvailableSeats1, flight1.AvailableSeats);
            Assert.Equal(Date1, flight1.Date);
            Assert.Equal(BasePrice1, flight1.BasePrice);

            FlightResponseDto flight2 = response.Flights.Single(x => x.Id.ToString() == Guid2);
            Assert.Equal(DepartureLocation2, flight2.DepartureLocation);
            Assert.Equal(ArrivalLocation2, flight2.ArrivalLocation);
            Assert.Equal(IsSpecialOffer2, flight2.IsSpecialOffer);
            Assert.Equal(AvailableSeats2, flight2.AvailableSeats);
            Assert.Equal(Date2, flight2.Date);
            Assert.Equal(BasePrice2, flight2.BasePrice);
        }

        [Fact]
        public async Task GetFlightsReturnsOkObjectResponseAndOneFlightWhenQueryStringArgumentsMatchSingleFlight()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);

            // Act
            IActionResult result = await flightController.Get(DepartureLocation1, string.Empty, string.Empty, 0).ConfigureAwait(false);
            var parsedResult = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(parsedResult.StatusCode == 200);
            var response = Assert.IsType<FlightsResponse>((parsedResult.Value));

            Assert.Single(response.Flights);

            FlightResponseDto flight1 = response.Flights.Single(x => x.Id.ToString() == Guid1);
            Assert.Equal(DepartureLocation1, flight1.DepartureLocation);
            Assert.Equal(ArrivalLocation1, flight1.ArrivalLocation);
            Assert.Equal(IsSpecialOffer1, flight1.IsSpecialOffer);
            Assert.Equal(AvailableSeats1, flight1.AvailableSeats);
            Assert.Equal(Date1, flight1.Date);
            Assert.Equal(BasePrice1, flight1.BasePrice);
        }

        [Fact]
        public async Task GetFlightsReturnsOkObjectResponseAndNoFlightsWhenQueryStringArgumentsDontMatchAnyFlight()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);

            // Act
            IActionResult result = await flightController.Get(DepartureLocation3, ArrivalLocation3, Date3, 300).ConfigureAwait(false);
            var parsedResult = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(parsedResult.StatusCode == 200);
            var response = Assert.IsType<FlightsResponse>((parsedResult.Value));
            Assert.Empty(response.Flights);
        }

        [Fact]
        public async Task GetFlightByIdReturnsBadRequestWhenGuidIsNull()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);

            // Act
            IActionResult result = await flightController.Get(Guid.Empty).ConfigureAwait(false);
            var parsedResult = result as BadRequestResult;

            // Assert
            Assert.IsType<BadRequestResult>(result);
            Assert.True(parsedResult.StatusCode == 400);
        }

        [Fact]
        public async Task GetByGuidReturnsNotFoundResultWhenNoResults()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);

            // Act
            IActionResult result = await flightController.Get(new Guid(Guid3)).ConfigureAwait(false);
            var parsedResult = result as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.True(parsedResult.StatusCode == 404);
        }


        [Fact]
        public async Task GetFlightByIdReturnsOkObjectResultAndCorrectObjectWhenRecordFound()
        {
            // Arrange
            m_Flights.Add(new Flight
            {
                Id = new Guid(Guid3),
                DepartureLocation = DepartureLocation3,
                ArrivalLocation = ArrivalLocation3,
                Date = Date3,
                BasePrice = BasePrice3,
                AvailableSeats = AvailableSeats3,
                IsSpecialOffer = IsSpecialOffer3
            });
            var flightController = new FlightsController(m_FlightRepository);
            
            // Act
            IActionResult result = await flightController.Get(new Guid(Guid2)).ConfigureAwait(false);
            var parsedResult = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(parsedResult.StatusCode == 200);

            var response = Assert.IsType<FlightResponse>((parsedResult.Value));

            FlightResponseDto flight2 = response.Flight;
            Assert.Equal(DepartureLocation2, flight2.DepartureLocation);
            Assert.Equal(ArrivalLocation2, flight2.ArrivalLocation);
            Assert.Equal(IsSpecialOffer2, flight2.IsSpecialOffer);
            Assert.Equal(AvailableSeats2, flight2.AvailableSeats);
            Assert.Equal(Date2, flight2.Date);
            Assert.Equal(BasePrice2, flight2.BasePrice);
        }

        #endregion

        #region PostFlight

        [Fact]
        public async Task PostReturnsOkObjectResultWhenFlightIsAdded()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);
            var flightDto = new FlightRequestDto
            {
                DepartureLocation = DepartureLocation3,
                ArrivalLocation = ArrivalLocation3,
                Date = Date3,
                BasePrice = BasePrice3,
                AvailableSeats = AvailableSeats3,
                IsSpecialOffer = IsSpecialOffer3
            };
            var flightRequest = new FlightRequest { Flight = flightDto };

            // Act
            IActionResult result = await flightController.Post(flightRequest).ConfigureAwait(false);
            var parsedResult = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(parsedResult.StatusCode == 200);

            var response = Assert.IsType<FlightResponse>((parsedResult.Value));
            var flightResponse = Assert.IsType<FlightResponseDto>(response.Flight);
            Assert.NotEqual(Guid.Empty, flightResponse.Id);
            Assert.Equal(DepartureLocation3, flightResponse.DepartureLocation);
            Assert.Equal(ArrivalLocation3, flightResponse.ArrivalLocation);
            Assert.Equal(Date3, flightResponse.Date);
            Assert.Equal(BasePrice3, flightResponse.BasePrice);
            Assert.Equal(AvailableSeats3, flightResponse.AvailableSeats);
            Assert.Equal(IsSpecialOffer3, flightResponse.IsSpecialOffer);
        }

        [Fact]
        public async Task PostReturnsBadRequestResultWhenFlightIsNull()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);
            var flightRequest = new FlightRequest { Flight = null };

            // Act
            IActionResult result = await flightController.Post(flightRequest).ConfigureAwait(false);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region PutFlight

        [Fact]
        public async Task PutReturnsNotFoundResponseWhenIdDoesNotExist()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);
            var flightDto = new FlightRequestDto
            {
                DepartureLocation = DepartureLocation3,
                ArrivalLocation = ArrivalLocation3,
                Date = Date3,
                BasePrice = BasePrice3,
                AvailableSeats = AvailableSeats3,
                IsSpecialOffer = IsSpecialOffer3
            };
            var flightRequest = new FlightRequest { Flight = flightDto };

            // Act
            IActionResult result = await flightController.Put(new Guid(Guid3), flightRequest).ConfigureAwait(false);
            var parsedResult = result as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.True(parsedResult.StatusCode == 404);
        }

        [Fact]
        public async Task PutReturnsOkResponseWhenExistingFlightIsUpdated()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);
            var flightDto = new FlightRequestDto
            {
                DepartureLocation = DepartureLocation3,
                ArrivalLocation = ArrivalLocation3,
                Date = Date3,
                BasePrice = BasePrice3,
                AvailableSeats = AvailableSeats3,
                IsSpecialOffer = IsSpecialOffer3
            };
            var flightRequest = new FlightRequest { Flight = flightDto };
            var flightGuid = new Guid(Guid2);

            // Act
            IActionResult result = await flightController.Put(flightGuid, flightRequest).ConfigureAwait(false);
            var parsedResult = result as OkObjectResult;

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.True(parsedResult.StatusCode == 200);

            var response = Assert.IsType<FlightResponse>(parsedResult.Value);
            Assert.IsType<FlightResponseDto>(response.Flight);

            Flight flightResponse = m_Flights.Single(x => x.Id == flightGuid);

            Assert.Equal(DepartureLocation3, flightResponse.DepartureLocation);
            Assert.Equal(ArrivalLocation3, flightResponse.ArrivalLocation);
            Assert.Equal(Date3, flightResponse.Date);
            Assert.Equal(BasePrice3, flightResponse.BasePrice);
            Assert.Equal(AvailableSeats3, flightResponse.AvailableSeats);
            Assert.Equal(IsSpecialOffer3, flightResponse.IsSpecialOffer);
        }

        [Fact]
        public async Task PutReturnsBadRequestWhenFlightIsNull()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);
            var flightRequest = new FlightRequest { Flight = null };

            // Act
            IActionResult result = await flightController.Put(new Guid(Guid3), flightRequest).ConfigureAwait(false);
            var parsedResult = result as BadRequestResult;

            // Assert
            Assert.IsType<BadRequestResult>(result);
            Assert.True(parsedResult.StatusCode == 400);
        }

        [Fact]
        public async Task PutReturnsNotFoundWhenIdIsNull()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);
            var flightDto = new FlightRequestDto
            {
                DepartureLocation = DepartureLocation3,
                ArrivalLocation = ArrivalLocation3,
                Date = Date3,
                BasePrice = BasePrice3,
                AvailableSeats = AvailableSeats3,
                IsSpecialOffer = IsSpecialOffer3
            };
            var flightRequest = new FlightRequest { Flight = flightDto };

            // Act
            IActionResult result = await flightController.Put(Guid.Empty, flightRequest).ConfigureAwait(false);
            var parsedResult = result as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.True(parsedResult.StatusCode == 404);
        }

        #endregion

        #region DeleteFlight

        [Fact]
        public async Task DeleteReturnsNotFoundResponseWhenIdDoesNotExist()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);

            // Act
            IActionResult result = await flightController.Delete(new Guid(Guid3)).ConfigureAwait(false);
            var parsedResult = result as NotFoundResult;

            // Assert
            Assert.IsType<NotFoundResult>(result);
            Assert.True(parsedResult.StatusCode == 404);
        }

        [Fact]
        public async Task DeleteReturnsOkResponseWhenExistingFlightDeleted()
        {
            // Arrange
            var flightController = new FlightsController(m_FlightRepository);
            var flightGuid = new Guid(Guid2);
            Flight flight = m_Flights.Single(x => x.Id == flightGuid);

            // Act
            IActionResult result = await flightController.Delete(flightGuid).ConfigureAwait(false);
            var parsedResult = result as OkResult;

            // Assert
            Assert.IsType<OkResult>(result);
            Assert.DoesNotContain(flight, m_Flights);
            Assert.Single(m_Flights);
        }

        #endregion
    }
}
