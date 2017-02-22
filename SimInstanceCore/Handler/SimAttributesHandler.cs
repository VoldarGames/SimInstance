using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimInstance
{
    public static class SimAttributesHandler<T>
    {
        public static Dictionary<Type,Action<PropertyInfo, T>> ActionDictionary { get; set; } = new Dictionary<Type, Action<PropertyInfo,T>>()
        {
            { typeof(SimRangeAttribute), (property, newEntity) => SimRangeRule<T>.ApplySimRangeRule(property,ref newEntity)}
        };

       
    }
}