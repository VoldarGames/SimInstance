using System;
using System.Reflection;
using SimInstanceLab.Managers.Helpers;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimRules.RuleAppliers.BaseClass;

namespace SimInstanceLab.SimRules.RuleAppliers
{
    public class SimRangeRuleApplier<T> : SimRuleApplier<T>
    {
        public override void ApplyRule(PropertyInfo property, ref T newEntity, SimAttribute simAttribute)
        {
            if (simAttribute == null) throw new NullReferenceException("SimAttribute is null");

            var minRangeValue = simAttribute.GetType().GetProperty("MinRange").GetValue(simAttribute);
            var maxRangeValue = simAttribute.GetType().GetProperty("MaxRange").GetValue(simAttribute);
            property.SetValue(newEntity, GetRandomRange(minRangeValue, maxRangeValue));
        }

        private static int GetRandomRange(object min, object max)
        {
            var rand = RandomSeedHelper.Random;
            return rand.Next((int)min, (int)max);
        }
    }
}