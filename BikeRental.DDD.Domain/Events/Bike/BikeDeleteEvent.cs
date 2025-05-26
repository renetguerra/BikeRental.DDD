using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.Events
{
    /// <summary>
    /// Event that notifies the deletion of an bike
    /// </summary>
    public class BikeDeleteEvent : DomainEvent
    {
        public BikeDeleteEvent(Bike bike, string msgNotification)
        {
            Bike = bike;
            MsgNotification = msgNotification;
        }

        public Bike Bike { get; set; }
    }
}
