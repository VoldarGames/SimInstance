using System;

namespace SimInstance
{
    public class SimRule<T>
    {
        public Type EntityType;
        public string PropertyName;
        public Type PropertyType;
        public SimAttribute SimAttribute;

        public SimRule(string propertyName, Type propertyType, SimAttribute simAttribute)
        {
            EntityType = typeof(T);
            PropertyName = propertyName;
            PropertyType = propertyType;
            SimAttribute = simAttribute;
        }

    }
}