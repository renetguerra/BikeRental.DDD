using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.Events
{
    public class UserCreateEvent : DomainEvent
    {
        public User User { get; set; }
        public UserCreateEvent(User user, string msgNotification)
        {
            User = user;
            MsgNotification = msgNotification;
        }
    }
}
