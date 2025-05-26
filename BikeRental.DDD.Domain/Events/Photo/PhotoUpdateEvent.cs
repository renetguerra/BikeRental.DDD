using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeRental.DDD.Domain.Events.Photo
{
    public class PhotoUpdateEvent : DomainEvent
    {
        public BikeRental.DDD.Domain.Entities.UserPhoto UserPhoto { get; set; }
        public BikeRental.DDD.Domain.Entities.BikePhoto BikePhoto { get; set; }
        public PhotoUpdateEvent(BikeRental.DDD.Domain.Entities.UserPhoto userPhoto, string msgNotification)
        {
            UserPhoto = userPhoto;
            MsgNotification = msgNotification;
        }

        public PhotoUpdateEvent(BikeRental.DDD.Domain.Entities.BikePhoto bikePhoto, string msgNotification)
        {
            BikePhoto = bikePhoto;
            MsgNotification = msgNotification;
        }
    }    
}
