using System;
using System.Collections.Generic;
using System.Linq;
using TrackZero.Abstract;
using TrackZero.Extensions;

namespace TrackZero.DataTransfer
{
    public class Event
    {
        private Event()
        {

        }

        public Event(string emitterType,
                     object emitterId,
                     string eventName,
                     object id = default,
                     DateTime? startTime = default,
                     Dictionary<string, object> customAttributes = default,
                     IEnumerable<EntityReference> targets = default,
                     DateTime? endTime = default)
                : this(new EntityReference(emitterType, emitterId),
                      eventName,
                      id,
                      startTime,
                      customAttributes,
                      targets,
                      endTime)
        {

        }

        public Event(string emitterType,
                     object emitterId,
                     string name)
            : this(emitterType, emitterId, name, default, DateTime.UtcNow, null, null, null)
        {

        }

        public Event(EntityReference emitter, string name, object id = default, DateTime? startTime = default, Dictionary<string, object> attributes = default, IEnumerable<EntityReference> targets = default, DateTime? endTime = default)
        {
            Emitter = emitter;
            Id = id ?? Guid.NewGuid();
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            CustomAttributes = attributes ?? new Dictionary<string, object>();
            Targets = targets?.ToList() ?? new List<EntityReference>();
        }

        public Event AddEntityReferencedAttribute(string attributeName, string type, object id)
        {
            CustomAttributes.TryAdd(attributeName, new EntityReference(type, id));
            return this;
        }
        public Event AddAttribute(string attributeName, object value)
        {
            CustomAttributes.TryAdd(attributeName, value);
            return this;
        }

        public Event AddTarget(string type, object id)
        {
            Targets.Add(new EntityReference(type, id));
            return this;
        }


        public EntityReference Emitter { get; set; }
        public object Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Dictionary<string, object> CustomAttributes { get; }
        public List<EntityReference> Targets { get; }

        internal void ValidateAndCorrect()
        {
            Id = Id.ValidateTypeForPremitiveValue();
            Emitter.Id = Emitter.Id.ValidateTypeForPremitiveValue();

            foreach (var cAttribute in this.CustomAttributes.ToList())
            {
                CustomAttributes[cAttribute.Key] = cAttribute.Value.ValidateTypeForPremitiveValueOrReferenceType();
            }

            foreach(var target in Targets)
            {
                target.Id = target.Id.ValidateTypeForPremitiveValue();
            }

        }
    }

}
