using System;

namespace SimInstanceLab.SimAttributes.Interfaces
{
    public interface ISimAttribute
    {
        Type[] GetParameterTypes();
        object[] GetParameterValues();
    }
}