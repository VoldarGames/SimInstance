using System;
using System.Collections.Generic;
using System.Linq;
using SimInstanceLab.SimRules.PrimaryKeyMap;

namespace SimInstanceLab.Managers
{
    public class SimMemoryProvider : ISimProvider
    {
        private readonly Dictionary<Type, List<object>> _container = new Dictionary<Type, List<object>>();

        public void Add(Type type, List<object> entities)
        {
            _container.Add(type, entities);
        }

        public List<object> GetAll(Type type)
        {
            if (!_container.ContainsKey(type)) throw new SimDontExistTypeInContainerException(type);
            return _container[type];
        }


        public object GetById(Type type, int id)
        {
            var containerPropertyType = _container[type]
                                        .FirstOrDefault()
                                        .GetType()
                                        .GetProperty(SimPrimaryKeyMap.GetPrimaryKeyPropertyName(type));
            return _container[type].AsParallel().Single(o => (int)containerPropertyType.GetValue(o) == id);
        }

        public bool ContainsKey(Type propertyType)
        {
            return _container.ContainsKey(propertyType);
        }

        public int GetCount(Type type)
        {
            return _container[type].Count;
        }
        internal class SimDontExistTypeInContainerException : Exception
        {
            public SimDontExistTypeInContainerException(Type type) : base($"{type.FullName} doesn't exist in container. Did you add the profile?") { }
        }
    }
}