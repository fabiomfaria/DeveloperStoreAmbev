using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(ILogger<EventPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync<T>(T @event) where T : class
        {
            var eventName = @event.GetType().Name;
            var eventData = JsonSerializer.Serialize(@event);

            _logger.LogInformation("EVENT PUBLISHED: {EventName} at {Timestamp}", eventName, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
            _logger.LogInformation("EVENT PAYLOAD: {EventData}", eventData);

            return Task.CompletedTask;
        }
    }
}