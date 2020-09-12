using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrackZero.Abstract;

namespace TrackZero.Extensions
{
    internal static class ObjectExtensions
    {
        static Type[] allowedTypes = { typeof(int), typeof(long), typeof(double), typeof(float), typeof(decimal), 
                                       typeof(string), typeof(DateTime), typeof(DateTimeOffset), typeof(bool), typeof(Guid),
                                       typeof(int?), typeof(long?), typeof(double?), typeof(float?), typeof(decimal?), 
                                       typeof(string), typeof(DateTime?), typeof(DateTimeOffset?), typeof(bool?), typeof(Guid?)};
        internal static void ValidatePremitiveValueOrReferenceType(this object obj)
        {

            if (obj != default && !allowedTypes.Contains(obj.GetType()) && !(obj is IEntityReference))
            {
                throw new InvalidOperationException($"Type {obj.GetType().Name} is not a premitive type. Only premitive types and IEntityReference objects are allowed");
            }
        }

        internal static void ValidatePremitiveValue(this object obj)
        {

            if (obj != default && !allowedTypes.Contains(obj.GetType()))
            {
                throw new InvalidOperationException($"Type {obj.GetType().Name} is not a premitive type. Only premitive types and IEntityReference objects are allowed");
            }
        }
    }
}
