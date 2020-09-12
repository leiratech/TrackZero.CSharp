using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TrackZero.Abstract;
using TrackZero.Extensions;

namespace TrackZero.DataTransfer
{
    public class Entity : IEntityReference
    {
        private Entity()
        {

        }

        /// <summary>
        /// Creates a new Entity Object
        /// </summary>
        /// <param name="id">The id of the object, this Id is your own Id. It must be premitive type (ie. numeric, string, Guid)</param>
        /// <param name="type">The name of the entity type (ie. Car, Driver, User...etc)</param>
        /// <param name="customAttributes">Any custom attributes you would like to include, the value can be a premitive type or an IEntityReference</param>
        /// <param name="creationTime">The time when this entity was created. Default is DateTime.UtcNow</param>
        /// <param name="ipAddress">The ip address of the entity creator, this is useful if you are tracking users, this will translate into a location in the dashboard.</param>
        public Entity(object id, string type, Dictionary<string, object> customAttributes = default, DateTime creationTime = default, string ipAddress = default)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"'{nameof(type)}' cannot be null or whitespace", nameof(type));
            }

            id.ValidatePremitiveValue();
            foreach (var kvp in CustomAttributes)
            {
                kvp.Value.ValidatePremitiveValueOrReferenceType();
            }

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
