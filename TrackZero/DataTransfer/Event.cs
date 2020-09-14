using System;
using System.Collections.Generic;
using System.Linq;
using TrackZero.Extensions;

namespace TrackZero.DataTransfer
{
    public class Event
    {
        private Event()
        {

        }

        public Event(EntityReference emitter, string name, IEnumerable<EntityReference> targets, object id = default, DateTime? startTime = default, DateTime? endTime = default, Dictionary<string, object> customAttributes = default, string ipAddress = default)
        {
            emitter.Id.ValidatePremitiveValue();
            if (emitter.Id == null) throw new ArgumentNullException(nameof(emitter.Id));
            if (string.IsNullOrWhiteSpace(emitter.Type)) throw new ArgumentNullException(nameof(emitter.Type));
            Emitter = emitter;

            id.ValidatePremitiveValue();
            Id = id ?? Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;

            StartTime = startTime;

            EndTime = endTime;

            CustomAttributes = customAttributes ?? new Dictionary<string, object>();

            if (targets == null || targets.Count() == 0)
            {
                throw new ArgumentNullException(nameof(name));
            }
            else
            {
                foreach (var target in targets)
                {
                    target.Id.ValidatePremitiveValue();
                    if (target.Id == null) throw new ArgumentNullException(nameof(target.Id));
                    if (string.IsNullOrWhiteSpace(target.Type)) throw new ArgumentNullException(nameof(target.Type));
                }
            }
            Targets = targets;
            IpAddress = ipAddress;
        }




        public EntityReference Emitter { get; set; }
        public object Id { get; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Dictionary<string, object> CustomAttributes { get; }
        public IEnumerable<EntityReference> Targets { get; }
        public string IpAddress { get; set; }

    }

}
