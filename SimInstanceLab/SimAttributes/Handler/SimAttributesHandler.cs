using System;
using System.Collections.Generic;
using System.Reflection;
using SimInstanceLab.SimRules.RuleAppliers;

namespace SimInstanceLab.SimAttributes.Handler
{
    public static class SimAttributesHandler<T>
    {
        public static Dictionary<Type,Action<PropertyInfo, T>> ActionDictionary { get; set; } = new Dictionary<Type, Action<PropertyInfo,T>>()
        {
            { typeof(SimRangeAttribute), (property, newEntity) => new SimRangeRuleApplier<T>().ApplyRule(property,ref newEntity)},
            {typeof(SimRegexAttribute),(property, newEntity) => new SimRegexRuleApplier<T>().ApplyRule(property,ref newEntity)}
        };

       
    }
}