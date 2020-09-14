using System;
using TrackZero.Abstract;
using TrackZero.Extensions;

namespace TrackZero.DataTransfer
{
    public class EntityReference : IEntityReference
    {
        /// <summary>
        /// Creates new reference to be used in events or entities.
        /// </summary>
        /// <param name="id">The id of the emitter entity, this Id is your own generated Id. It must be premitive type (ie. numeric, string, Guid)</param>
        /// <param name="type">The name of the entity type (ie. Car, Driver, User...etc)</param>
        public EntityReference(string type, object id)
        {
            id.ValidatePremitiveValue();
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        private EntityReference()
        {

        }


        public object Id { get; set; }
        public string Type { get; set; }
    }

}
