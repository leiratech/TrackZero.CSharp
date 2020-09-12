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
        /// <param name="customAttributes">Any custom attributes you would like to include, the value can be a premitive type or an EntityReference</param>
        /// <param name="creationTime">The time when this entity was created. Default is DateTime.UtcNow</param>
        /// <param name="ipAddress" >The ip address of the entity creator, this is useful if you are tracking users, it will translate into a location in the dashboard.</param>
        /// <exception cref="ArgumentNullException">Thrown when id is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when id or any customAttribute are not premitive type.</exception>
        /// <returns>Returns the entity you created or throws exception on error</returns>
        public Entity(object id, string type, Dictionary<string, object> customAttributes = default, DateTime? creationTime = default, string ipAddress = default)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException($"'{nameof(type)}' cannot be null or whitespace", nameof(type));
            }
            Type = type;

            id.ValidatePremitiveValue();
            this.Id = id ?? throw new ArgumentNullException(nameof(id));

            CustomAttributes = customAttributes ?? new Dictionary<string, object>();
            foreach (var kvp in CustomAttributes)
            {
                kvp.Value.ValidatePremitiveValueOrReferenceType();
            }

            CreationTime = creationTime == default ? DateTime.UtcNow : creationTime.Value;
            IpAddress = ipAddress;
        }

        public Dictionary<string, object> CustomAttributes { get; }
        public DateTime CreationTime { get; set; }
        public string IpAddress { get; set; }
        public string Type { get; set; }
        public object Id { get; }
    }


}
