using System;
using System.Reflection;
using Fare;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimRules.RuleAppliers.BaseClass;

namespace SimInstanceLab.SimRules.RuleAppliers
{
    public class SimRegexRuleApplier<T> : SimRuleApplier<T>
    {
        public override void ApplyRule(PropertyInfo property, ref T newEntity, SimAttribute simAttribute)
        {
            if (simAttribute == null) throw new NullReferenceException("SimAttribute is null");

            var regularExpression = simAttribute.GetType().GetProperty("RegularExpression").GetValue(simAttribute) as string;
          
            var xeger = new Xeger(regularExpression);
            property.SetValue(newEntity,xeger.Generate());
        }
    }
}