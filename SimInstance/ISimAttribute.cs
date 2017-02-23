using System;

namespace SimInstance
{
    public interface ISimAttribute
    {
        Type[] GetParameterTypes();
        object[] GetParameterValues();
    }
}