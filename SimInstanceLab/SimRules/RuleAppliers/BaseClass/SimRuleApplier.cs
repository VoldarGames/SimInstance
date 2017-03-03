using System.Reflection;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimRules.RuleAppliers.BaseClass
{
    public abstract class SimRuleApplier<T>
    {
        public abstract void ApplyRule(PropertyInfo property, ref T newEntity, SimAttribute simAttribute);
    }
}