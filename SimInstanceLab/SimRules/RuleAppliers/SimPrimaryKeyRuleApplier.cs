using System.Reflection;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimRules.PrimaryKeyMap;
using SimInstanceLab.SimRules.RuleAppliers.BaseClass;

namespace SimInstanceLab.SimRules.RuleAppliers
{
    public class SimPrimaryKeyRuleApplier<T> : SimRuleApplier<T>
    {
        public override void ApplyRule(PropertyInfo property, ref T newEntity, SimAttribute simAttribute)
        {
            SimPrimaryKeyMap.AddNewPrimaryKey(typeof(T), property.Name);
        }

    }
}