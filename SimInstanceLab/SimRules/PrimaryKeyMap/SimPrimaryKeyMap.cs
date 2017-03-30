using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimInstanceLab.SimRules.PrimaryKeyMap
{
    public static class SimPrimaryKeyMap
    {
        private static readonly Dictionary<Type, string> Map = new Dictionary<Type, string>();

        /// <summary>
        /// Add new primary Key if it doesnt exist
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        public static void AddNewPrimaryKey(Type type, string propertyName)
        {
            if (!Map.ContainsKey(type))
            {
                Map.Add(type, propertyName);
            }
        }

        public static string GetPrimaryKeyPropertyName(Type type)
        {
            if (!Map.ContainsKey(type)) throw new SimPrimaryKeyNotDefinedException(type);
            return Map[type];
        }
        internal class SimPrimaryKeyNotDefinedException : Exception
        {
            public SimPrimaryKeyNotDefinedException(Type type) : base($"{type.FullName} was not defined as Primary Key in its profile.") { }
        }
    }
}
