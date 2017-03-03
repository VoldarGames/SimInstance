using System;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimRules
{
    public class SimRule<T> : ISimRule
    {
        public Type EntityType { get; set; }
        public string PropertyName { get; set; }
        public Type PropertyType { get; set; }
        public SimAttribute SimAttribute { get; set; }

        public SimRule(string propertyName, Type propertyType, SimAttribute simAttribute)
        {
            EntityType = typeof(T);
            PropertyName = propertyName;
            PropertyType = propertyType;
            SimAttribute = simAttribute;
        }

        
    }
}