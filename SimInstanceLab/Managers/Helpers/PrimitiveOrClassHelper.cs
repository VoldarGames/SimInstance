using System;
using System.Collections.Generic;
using System.Linq;

namespace SimInstanceLab.Managers.Helpers
{
    public class PrimitiveOrClassHelper
    {
        public static List<Type> PrimitiveTypes = new List<Type>()
        {
            typeof(string),
            typeof(DateTime),
            typeof(Nullable<>),
            typeof(Nullable),
            typeof(decimal)


        };

        public static bool IsPrimitive(Type propertyType)
        {
            if (propertyType.IsGenericType) return IsPrimitive(propertyType.GenericTypeArguments.FirstOrDefault());
            return PrimitiveTypes.Contains(propertyType) || propertyType.IsPrimitive;
        }
    }
}