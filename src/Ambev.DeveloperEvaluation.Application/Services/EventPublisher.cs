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

            _logger.LogInformation($"Event Published: {eventName}");
            _logger.LogInformation($"Event Data: {eventData}");

            // Aqui seria o ponto de integração com um Message Broker como RabbitMQ ou Kafka
            // Implementação simplificada conforme solicitado nos requisitos

            return Task.CompletedTask;
        }
    }
}