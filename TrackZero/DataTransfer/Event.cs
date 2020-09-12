using System;
using System.Collections.Generic;
using TrackZero.Abstract;

namespace TrackZero.DataTransfer
{
    public class Event : IEntityReference
    {
        private Event()
        {

        }

        public Event(object id, string type, Dictionary<string, object> customAttributes = default, DateTime creationTime = default, string ipAddress = default)
        {
            this.Id = id;
            Type = type;
            CustomAttributes = customAttributes ?? new Dictionary<string, object>();
            CreationTime = creationTime == default ? DateTime.UtcNow : creationTime;
            IpAddress = ipAddress;
        }

        public Dictionary<string, object> CustomAttributes { get; }
        public DateTime CreationTime { get; set; }
        public string IpAddress { get; set; }
        public string Type { get; set; }
        public object Id { get; set; }

    }


}
