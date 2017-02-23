using System.Reflection;

namespace SimInstance
{
    public abstract class SimRuleApplier<T>
    {
        public abstract void ApplyRule(PropertyInfo property, ref T newEntity);
    }
}