using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrackZero.Abstract;

namespace TrackZero.Extensions
{
    internal static class ObjectExtensions
    {
        static Type[] allowedTypes = { typeof(int), typeof(int?),
                                       typeof(long), typeof(long?),
                                       typeof(double), typeof(double?),
                                       typeof(float), typeof(float?),
                                       typeof(decimal), typeof(decimal?),
                                       typeof(string), 
                                       typeof(DateTime), typeof(DateTime?),
                                       typeof(DateTimeOffset), typeof(DateTimeOffset?),
                                       typeof(bool), typeof(bool?),
                                       typeof(Guid),typeof(Guid?)};

        internal static object ValidateTypeForPremitiveValueOrReferenceType(this object obj)
        {
            if (obj != default && !allowedTypes.Contains(obj.GetType()) && !(obj is IEntityReference))
            {
                throw new InvalidOperationException($"Type {obj.GetType().Name} is not a premitive type. Only premitive types and IEntityReference objects are allowed");
            }

            if (obj is DateTime dt)
            {
                obj = new DateTimeOffset(dt).UtcDateTime;
            }
            else if (obj is DateTimeOffset dto)
            {
                obj = dto.UtcDateTime;
            }

            return obj;
        }

        internal static object ValidateTypeForPremitiveValue(this object obj)
        {
            if (obj != default && !allowedTypes.Contains(obj.GetType()))
            {
                throw new InvalidOperationException($"Type {obj.GetType().Name} is not a premitive type. Only premitive types and IEntityReference objects are allowed");
            }

            if (obj is DateTime dt)
            {
                obj = new DateTimeOffset(dt).UtcDateTime;
            }
            else if (obj is DateTimeOffset dto)
            {
                obj = dto.UtcDateTime;
            }

            return obj;

        }
    }
}
