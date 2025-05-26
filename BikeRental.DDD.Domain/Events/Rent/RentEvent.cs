using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.Events.Rent
{
    /// <summary>
    /// Event that advice when the like has been created
    /// </summary>
    public class RentEvent : DomainEvent
    {
        public BikeRental.DDD.Domain.Entities.Rental Rent { get; set; }
        public RentEvent(BikeRental.DDD.Domain.Entities.Rental rent, string msgNotification) 
        {
            Rent = rent;
            MsgNotification = msgNotification;
        }
    }
}
