using System.Reflection;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimRules.NullPropertiesMap;
using SimInstanceLab.SimRules.RuleAppliers.BaseClass;

namespace SimInstanceLab.SimRules.RuleAppliers
{
    public class SimIsNullRuleApplier<T> : SimRuleApplier<T>
    {
        public override void ApplyRule(PropertyInfo property, ref T newEntity, SimAttribute simAttribute)
        {
            SimNullPropertiesMap.NullProperties.Add(property);
            property.SetValue(newEntity, null);
        }
    }
}