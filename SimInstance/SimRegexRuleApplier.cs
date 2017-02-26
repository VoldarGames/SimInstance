using System.Reflection;
using Fare;

namespace SimInstance
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