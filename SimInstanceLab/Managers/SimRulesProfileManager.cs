using System;
using System.Collections.Generic;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstanceLab.Managers
{
    public static class SimRulesProfileManager
    {
        public static Dictionary<Type, IAbstractSimRulesProfile> ProfilesDictionary { get; } = new Dictionary<Type, IAbstractSimRulesProfile>();
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

        
    }
}
