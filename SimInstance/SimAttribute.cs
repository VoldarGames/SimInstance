using System;

namespace SimInstance
{
    public abstract class SimAttribute : Attribute, ISimAttribute{
        public abstract Type[] GetParameterTypes();
        public abstract object[] GetParameterValues();
    }
}