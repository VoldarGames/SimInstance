using System;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimAttributes
{
    public class SimIsNullAttribute : SimAttribute
    {
        public override Type[] GetParameterTypes()
        {
            return null;
        }

        public override object[] GetParameterValues()
        {
            return null;
        }
    }
}