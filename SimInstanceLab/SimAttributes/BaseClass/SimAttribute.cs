using System;
using SimInstanceLab.SimAttributes.Interfaces;

namespace SimInstanceLab.SimAttributes.BaseClass
{
    public abstract class SimAttribute : Attribute, ISimAttribute
    {
        public abstract Type[] GetParameterTypes();
        public abstract object[] GetParameterValues();
    }
}