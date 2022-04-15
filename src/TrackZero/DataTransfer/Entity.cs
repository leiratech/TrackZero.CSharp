using System;
using System.Collections.Generic;
using TrackZero.Abstract;
using TrackZero.Extensions;

namespace TrackZero.DataTransfer
{
    /// <summary>
    /// Each Entity represents a data object in TrackZero.
    /// </summary>
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

        /// <summary>
        /// Creates a new Entity with a sepcific type and id.
        /// </summary>
        /// <param name="type">The type of the Entity</param>
        /// <param name="id">The id of the Entity.</param>
        public Entity(string type, object id)
        {
            Type = type;
            Id = id;
        }

        /// <summary>
        /// Adds an attribute that references another entity.
        /// </summary>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="type">The Referenced Entity Type</param>
        /// <param name="id">The Referenced Entity Id, value can be a string, int, long, double or Guid</param>
        /// <returns></returns>
        public Entity AddEntityReferencedAttribute(string attributeName, string type, object id)
        {
            if (CustomAttributes.ContainsKey(attributeName))
            {
                ((List<EntityReference>)CustomAttributes[attributeName]).Add(new EntityReference(type, id.ValidateTypeForPremitiveValue()));
            }
            else
            {
                CustomAttributes.TryAdd(attributeName, new List<EntityReference>() { new EntityReference(type, id.ValidateTypeForPremitiveValue()) });
            }
            return this;
        }

        /// <summary>
        /// Adds an attribute with a value.
        /// </summary>
        /// <param name="attributeName">The attribute name</param>
        /// <param name="value">The value of the attribute, this can be any premitive value</param>
        /// <returns></returns>
        public Entity AddAttribute(string attributeName, object value)
        {
            CustomAttributes.TryAdd(attributeName, value.ValidateTypeForPremitiveValue());
            return this;
        }

        /// <summary>
        /// Please use the provided methods to add Attributes. Adding directly might result in data rejection.
        /// </summary>
        public Dictionary<string, object> CustomAttributes { get; } = new Dictionary<string, object>();

        /// <summary>
        /// The Type of the entity.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The id of the entity.
        /// </summary>
        public object Id { get; private set; }

        /// <summary>
        /// Please use AddAutomaticallyTranslatedGeoPoint(double, double) to add a Geographically Translated Point. Adding directly might result in data rejection. 
        /// </summary>
        public GeographyAutomaticReferencing AutoGeography { get; set; }

        internal void ValidateAndCorrect()
        {
            Id = Id.ValidateTypeForPremitiveValue();

            foreach (var cAttribute in this.CustomAttributes)
            {
                CustomAttributes[cAttribute.Key] = cAttribute.Value.ValidateTypeForPremitiveValueOrReferenceType();
            }


        }

        /// <summary>
        /// Automatically Translates a GeoPoint (Lat, Long) to Country and State and links them as Referenced Entity.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public Entity AddAutomaticallyTranslatedGeoPoint(double latitude, double longitude, bool includeActualGeoPointInEntity = false)
        {
            this.AutoGeography = new GeographyAutomaticReferencing(latitude, longitude);
            if (includeActualGeoPointInEntity)
            {
                AddAttribute("GeoPoint (Automatic)", $"{latitude}, {longitude}");
            }
            return this;
        }
    }
}
