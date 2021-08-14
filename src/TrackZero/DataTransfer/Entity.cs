using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using TrackZero.Abstract;
using TrackZero.Extensions;

namespace TrackZero.DataTransfer
{
    public class Entity : IEntityReference
    {
        /// <summary>
        /// Creates a new Entity Object
        /// </summary>
        /// <param name="id">The id of the object, this Id is your own Id. It must be premitive type (ie. numeric, string, Guid)</param>
        /// <param name="type">The name of the entity type (ie. Car, Driver, User...etc)</param>
        /// <param name="attributes">Any custom attributes you would like to include, the value can be a premitive type or an EntityReference</param>
        /// <exception cref="ArgumentNullException">Thrown when id is null</exception>
        /// <exception cref="InvalidOperationException">Thrown when id or any customAttribute are not premitive type.</exception>
        /// <returns>Returns the entity you created or throws exception on error</returns>
        public Entity(string type, object id, Dictionary<string, object> attributes = default)
        {
            Type = type;
            Id = id;
            CustomAttributes = attributes ?? new Dictionary<string, object>();
        }

        public Entity(string type, object id)
        {
            Type = type;
            Id = id;
        }

        public Entity AddEntityReferencedAttribute(string attributeName, string type, object id)
        {
            CustomAttributes.Add(attributeName, new EntityReference(type, id.ValidateTypeForPremitiveValue()));
            return this;
        }

        public Entity AddAttribute(string attributeName, object value)
        {
            CustomAttributes.Add(attributeName, value.ValidateTypeForPremitiveValueOrReferenceType());
            return this;
        }

        public Dictionary<string, object> CustomAttributes { get; } = new Dictionary<string, object>();
        public string Type { get; set; }
        public object Id { get; private set; }

        internal void ValidateAndCorrect()
        {
            Id = Id.ValidateTypeForPremitiveValue();

            foreach (var cAttribute in this.CustomAttributes)
            {
                CustomAttributes[cAttribute.Key] = cAttribute.Value.ValidateTypeForPremitiveValueOrReferenceType();
            }


        }
    }


}
