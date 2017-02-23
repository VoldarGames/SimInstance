using System;

namespace SimInstance
{
    public class SimRule
    {
        public string PropertyName;
        public Type PropertyType;
        public SimAttribute SimAttribute;

        public SimRule(string propertyName, Type propertyType, SimAttribute simAttribute)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            SimAttribute = simAttribute;
        }

    }
}