using System;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimAttributes
{
    public class SimNavigationAttribute : SimAttribute
    {
        public string ForeignKeyPropertyName { get; set; }

        public SimNavigationAttribute(string foreignKeyPropertyName)
        {
            ForeignKeyPropertyName = foreignKeyPropertyName;
        }
        public override Type[] GetParameterTypes()
        {
            return new[] { typeof(string) };
        }

        public override object[] GetParameterValues()
        {
            return new object[] { ForeignKeyPropertyName };
        }
    }
}