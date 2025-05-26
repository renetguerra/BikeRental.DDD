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
    /// Manejador del evento que notifica la eliminación de un usuario.
    /// </summary>
    public class ItemRemovedEventHandler : INotificationHandler<UserDeleteEvent>
    {
        private readonly ILogger<ItemRemovedEventHandler> _logger;

        public ItemRemovedEventHandler(ILogger<ItemRemovedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserDeleteEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("############## BikeRental APIs Domain Event: {DomainEvent} #################", notification.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
