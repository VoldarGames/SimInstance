using System;
using System.Collections.Generic;

namespace SimInstanceLab.SimRules.NavigationMap
{
    public static class SimNavigationMap
    {
        private static readonly Dictionary<Tuple<Type,string>, string> Map = new Dictionary<Tuple<Type, string>, string>();

        
        public static void AddNewNavigation(Type type, string navigationPropertyName, string foreignKeyPropertyName)
        {
            var tuple = new Tuple<Type, string>(type,navigationPropertyName);
            if (!Map.ContainsKey(tuple))
            {
                Map.Add(tuple, foreignKeyPropertyName);
            }
        }

        public static string GetNavigationForeignKeyPropertyName(Type parentType, string navigationPropertyName)
        {
            var tuple = new Tuple<Type,string>(parentType, navigationPropertyName);
            if (!Map.ContainsKey(tuple)) throw new SimNavigationNotDefinedException(parentType, navigationPropertyName);
            return Map[tuple];
        }
        internal class SimNavigationNotDefinedException : Exception
        {
            public SimNavigationNotDefinedException(Type type,string navigationPropertyName) : base($" {navigationPropertyName} was not defined as Navigation in {type.FullName} profile.") { }
        }

        public static bool Exist(Type parentType, string navigationPropertyName)
        {
            var tuple = new Tuple<Type, string>(parentType, navigationPropertyName);
            return Map.ContainsKey(tuple);
        }
    }
}
