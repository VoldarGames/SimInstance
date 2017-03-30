using System;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimAttributes
{
    public class SimForeignKeyAttribute : SimAttribute
    {
        public Type ParentType { get; set; }

        public SimForeignKeyAttribute(Type parentType)
        {
            ParentType = parentType;
        }
        public override Type[] GetParameterTypes()
        {
            return new[] { typeof(Type) };
        }

        public override object[] GetParameterValues()
        {
            return new object[] { ParentType };
        }
    }
}