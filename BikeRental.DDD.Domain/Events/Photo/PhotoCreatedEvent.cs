using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.Events.Photo
{
    /// <summary>
    /// Event that advice when the photo has been created
    /// </summary>
    public class PhotoCreatedEvent : DomainEvent
    {
        public BikeRental.DDD.Domain.Entities.UserPhoto UserPhoto { get; set; }
        public BikeRental.DDD.Domain.Entities.BikePhoto BikePhoto { get; set; }
        public PhotoCreatedEvent(BikeRental.DDD.Domain.Entities.UserPhoto userPhoto, string msgNotification) 
        { 
            UserPhoto = userPhoto;
            MsgNotification = msgNotification;
        }
        public PhotoCreatedEvent(BikeRental.DDD.Domain.Entities.BikePhoto bikePhoto, string msgNotification)
        {
            BikePhoto = bikePhoto;
            MsgNotification = msgNotification;
        }
    }
}
