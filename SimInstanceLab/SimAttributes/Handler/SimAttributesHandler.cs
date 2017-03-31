using System;
using System.Collections.Generic;
using System.Reflection;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimRules.RuleAppliers;

namespace SimInstanceLab.SimAttributes.Handler
{
    public static class SimAttributesHandler<T>
    {
        public static Dictionary<Type, Action<PropertyInfo, T, SimAttribute>> ActionDictionary { get; set; } = new Dictionary<Type, Action<PropertyInfo, T, SimAttribute>>()
        {
            {typeof(SimRangeAttribute), (property, newEntity, simAttribute) => new SimRangeRuleApplier<T>().ApplyRule(property,ref newEntity,simAttribute)},
            {typeof(SimRegexAttribute),(property, newEntity, simAttribute) => new SimRegexRuleApplier<T>().ApplyRule(property,ref newEntity,simAttribute)},
            {typeof(SimIgnoreAttribute), (property, newEntity, simAttribute) => { }},
            {typeof(SimNavigationAttribute), (property, newEntity, simAttribute) => new SimNavigationRuleApplier<T>().ApplyRule(property,ref newEntity,simAttribute)},
            {typeof(SimIsNullAttribute),(property, newEntity, simAttribute) => new SimIsNullRuleApplier<T>().ApplyRule(property,ref newEntity,simAttribute)},
            {typeof(SimPrimaryKeyAttribute),(property, newEntity, simAttribute) => new SimPrimaryKeyRuleApplier<T>().ApplyRule(property,ref newEntity,simAttribute)},
            {typeof(SimForeignKeyAttribute), (property, newEntity, simAttribute) => new SimForeignKeyRuleApplier<T>().ApplyRule(property,ref newEntity,simAttribute)},



        };


    }
}