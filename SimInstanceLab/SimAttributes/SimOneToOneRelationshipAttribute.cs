using System;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimAttributes
{
    public class SimOneToOneRelationshipAttribute : SimAttribute
    {
        public Type ParentType;
        public Type RelationType;
        public SimOneToOneRelationshipAttribute(Type parentType, Type relationType)
        {
            ParentType = parentType;
            RelationType = relationType;
        }

        public override Type[] GetParameterTypes()
        {
            return new[] { typeof(Type), typeof(Type) };
        }

        public override object[] GetParameterValues()
        {
            return new object[] { ParentType, RelationType };
        }
    }
}