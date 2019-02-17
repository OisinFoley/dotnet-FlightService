using EventDispatcher.Generic;
using FlightsService.Data.Abstract;
using FlightsService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace FlightsService.BackgroundWorker
{
    public class IncomingMessageService<T> : BackgroundService
    {
        private readonly IEventReceiver<EventMessage<Booking>> m_EventReceiver;
        private readonly ILogger m_Logger;
        private readonly IFlightRepository m_FlightRepository;
        private readonly IMessageRepository m_MessageRepository;

        public IncomingMessageService(ILogger<IncomingMessageService<T>> logger, IMessageRepository messageRepository, IEventReceiver<EventMessage<Booking>> receiver, IFlightRepository flightRepository)
        {
            m_Logger = logger;
            m_MessageRepository = messageRepository;
            m_FlightRepository = flightRepository;

            m_Logger.LogInformation($"IncomingMessageService initialising.");

            m_EventReceiver = receiver;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            m_EventReceiver.RegisterHandler(HandleReceivedMessage);

            await Task.CompletedTask;
        }

        private async Task HandleReceivedMessage(EventMessage<Booking> inboundBookingMessage)
        {
            switch (inboundBookingMessage.Header)
            {
                case "POST":
                    await UpdateFlightAndVerifyWasSuccessful(inboundBookingMessage.Content);
                    break;
                default:
                    break;
            }
        }

        private async Task<bool> UpdateFlightAndVerifyWasSuccessful(Booking booking)
        {
            try
            {
                var isSuccessfulUpdate = await Update(booking);
                if (isSuccessfulUpdate)
                {
                    m_Logger.LogInformation($"Flight Id {booking.FlightId} updated due to recently received booking with Booking Id {booking.Id}.");
                    return true;
                }
                else
                {
                    m_Logger.LogInformation($"No Flight was updated, Booking may be invalid.");
                    return false;
                }
            }
            catch (Exception e)
            {
                m_Logger.LogError($"Error updating Flight ${booking.FlightId} for Booking ${booking.Id}: Message ${e.Message}");
                throw;
            }
        }

        private async Task<bool> Update(Booking booking)
        {
            Flight flight = m_FlightRepository.Flights.FirstOrDefault(f => f.Id == booking.FlightId);

            if (flight != null)
            {
                flight.AvailableSeats--;
                await m_FlightRepository.UpdateAsync(flight);
                return true;
            }
            else
            {
                m_Logger.LogCritical($"No Flight Id found for id: {booking.FlightId}, for the Booking Id: {booking.Id}. ");
                return false;
            }
        }
    }
}
