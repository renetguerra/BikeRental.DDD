using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BikeRental.DDD.Application.EventHandlers.User
{
    /// <summary>
    /// Manejador del evento que notifica la modificación de la edad de un usuario.
    /// </summary>
    public class AgeUserChangedEventHandler : INotificationHandler<UserUpdateEvent>
    {
        private readonly ILogger<AgeUserChangedEventHandler> _logger;

        public AgeUserChangedEventHandler(ILogger<AgeUserChangedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserUpdateEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("############## BikeRental APIs Domain Event: {DomainEvent} #################", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
