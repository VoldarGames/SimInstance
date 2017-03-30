using System;
using System.Collections.Generic;
using System.Linq;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstanceLab.Managers
{
    public static class SimRulesProfileManager
    {


        public static Dictionary<Type, IAbstractSimRulesProfile> ProfilesDictionary { get; } = new Dictionary<Type, IAbstractSimRulesProfile>();
        public static List<Type> IgnoreTypeList = new List<Type>();

        public static void AddProfile<T>(IAbstractSimRulesProfile simRulesProfile)
        {
            if (!ProfilesDictionary.ContainsKey(typeof(T)))
            {
                ProfilesDictionary.Add(typeof(T), simRulesProfile);
            }
        }



        //TODO: Verify Profiles
        public static bool VerifyProfiles()
        {
            foreach (var profile in ProfilesDictionary)
            {
                foreach (var simRule in profile.Value.SimRules)
                {
                    //TODO: Verify SimAttribute values, count and more...
                }

            }
            return true;
        }


        public static void IgnoreAll<T>()
        {
            IgnoreTypeList.Add(typeof(T));
        }
        public static void IgnoreAll(Type t)
        {
            IgnoreTypeList.Add(t);
        }

        public static List<Type> GetAllIgnoredTypesFromProfiles()
        {
            var simRules = ProfilesDictionary.SelectMany(
                pair => pair.Value.SimRules.Where(rule => rule.SimAttribute.GetType() == typeof(SimIgnoreAttribute)));
            return simRules.Select(rule => rule.PropertyType).ToList();
        }
    }
}
