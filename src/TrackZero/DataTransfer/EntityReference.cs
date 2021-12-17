using System;
using TrackZero.Abstract;
using TrackZero.Extensions;

namespace TrackZero.DataTransfer
{
    /// <summary>
    /// Creates a relationship between two entities. Similar to a Master-Details / Details-Details in RDBMS.
    /// Please use AddEntityReferencedAttribute(string, string, object) to create such relationship to avoid data rejection.
    /// </summary>
    public class EntityReference : IEntityReference
    {
        /// <summary>
        /// Creates new reference to be used in events or entities.
        /// </summary>
        /// <param name="id">The id of the emitter entity, this Id is your own generated Id. It must be premitive type (ie. numeric, string, Guid)</param>
        /// <param name="type">The name of the entity type (ie. Car, Driver, User...etc)</param>
        public EntityReference(string type, object id)
        {
            Id = id;
            Type = type;
            Validate();
        }

        private EntityReference()
        {

        }

        /// <summary>
        /// Validates the object content.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void Validate()
        {
            Id.ValidateTypeForPremitiveValue();
            if (Id == default)
            {
                throw new ArgumentNullException(nameof(Id));
            }

            if (string.IsNullOrEmpty(Type) || string.IsNullOrWhiteSpace(Type))
            {
                throw new ArgumentNullException(nameof(Type));
            }
        }

        /// <summary>
        /// The id of the referenced entity.
        /// </summary>
        public object Id { get; set; }

        /// <summary>
        /// The type of the referenced entity.
        /// </summary>
        public string Type { get; set; }
    }
}
