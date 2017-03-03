using System.Reflection;
using Fare;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.RuleAppliers.BaseClass;

namespace SimInstanceLab.SimRules.RuleAppliers
{
    public class SimRegexRuleApplier<T> : SimRuleApplier<T>
    {
        public override void ApplyRule(PropertyInfo property, ref T newEntity)
        {
            var regexAttribute = property.GetCustomAttribute(typeof(SimRegexAttribute));
            var regularExpression = regexAttribute.GetType().GetProperty("RegularExpression").GetValue(regexAttribute) as string;
            var xeger = new Xeger(regularExpression);
            property.SetValue(newEntity,xeger.Generate());
        }
    }
}