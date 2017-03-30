using System;
using System.Collections.Generic;

namespace SimInstanceLab.Managers
{
    public interface ISimProvider
    {
        void Add(Type type, List<object> entities);

        object GetById(Type type, int id);
        bool ContainsKey(Type propertyType);
        List<object> GetAll(Type type);
        int GetCount(Type type);
    }

    
}