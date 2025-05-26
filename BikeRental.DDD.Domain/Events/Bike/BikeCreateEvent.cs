using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.Events
{
    public class BikeCreateEvent : DomainEvent
    {
        public Bike Bike { get; set; }
        public BikeCreateEvent(Bike bike, string msgNotification)
        {
            Bike = bike;
            MsgNotification = msgNotification;
        }
    }
}
