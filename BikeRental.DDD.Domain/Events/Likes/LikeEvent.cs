using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BikeRental.DDD.Domain.Entities;

namespace BikeRental.DDD.Domain.Events.Likes
{
    /// <summary>
    /// Event that advice when the like has been created
    /// </summary>
    public class LikeEvent : DomainEvent
    {
        public BikeRental.DDD.Domain.Entities.Like Like { get; set; }
        public LikeEvent(BikeRental.DDD.Domain.Entities.Like like, string msgNotification) 
        {
            Like = like;
            MsgNotification = msgNotification;
        }
    }
}
