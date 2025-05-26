using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.Events
{
    /// <summary>
    /// Event that notifies the deletion of an user
    /// </summary>
    public class UserDeleteEvent : DomainEvent
    {
        public UserDeleteEvent(User user, string msgNotification)
        {
            User = user;
            MsgNotification = msgNotification;
        }

        public User User { get; set; }
    }
}
