using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimInstanceLab.Managers.Helpers;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.AbstractProfile;
using SimInstanceLab.SimRules.PrimaryKeyMap;

namespace SimInstanceLab.Managers
{
    public abstract class Stage
    {
        public Stage UseSeed(int seed)
        {
            RandomSeedHelper.Seed = seed;
            return this;
        }
        public Stage ForceAction(Action action)
        {
            action?.Invoke();
            return this;
        }
        public Stage UseProfile<TProfileEntity>(AbstractSimRulesProfile<TProfileEntity> profile, bool forcePrimaryKey = false) where TProfileEntity : new()
        {
            SimRulesProfileManager.AddProfile<TProfileEntity>(profile);

            if (forcePrimaryKey)
            {

                var pkRule = SimRulesProfileManager.ProfilesDictionary[typeof(TProfileEntity)]
                    .SimRules
                    .FirstOrDefault(rule => rule.SimAttribute.GetType() == typeof(SimPrimaryKeyAttribute));

                if (pkRule == null) throw new SimPrimaryKeyMap.SimPrimaryKeyNotDefinedException(typeof(TProfileEntity));


                SimPrimaryKeyMap.AddNewPrimaryKey(typeof(TProfileEntity), pkRule.PropertyName);

            }
            return this;
        }

        public Stage UseProvider(ISimProvider provider)
        {
            SimContainer.Container = provider;
            return this;
        }

        public Stage IgnoreAllTypes(params Type[] types)
        {
            foreach (var type in types)
            {
                var method = typeof(SimRulesProfileManager).GetMethod("IgnoreAll", new Type[] { });
                MethodInfo generic;
                if (type.IsGenericType)
                {

                    generic = method.MakeGenericMethod(typeof(object));
                }
                else
                {
                    generic = method.MakeGenericMethod(type);
                }
                generic.Invoke(null, null);
            }
            return this;
        }

        public List<T> Execute<T>(Dictionary<Type, int> numberOfInstances = null)
        {
            if (numberOfInstances == null) numberOfInstances = new Dictionary<Type, int>();
            if (!numberOfInstances.ContainsKey(typeof(T))) throw new SimCantGenerateException($"In Execute Method from Stage {GetType().FullName} -> If Parent is included in numberOfInstances Dictionary it must have at least 1 instance to generate.");

            var ignoredTypes = SimRulesProfileManager.GetAllIgnoredTypesFromProfiles();

            var dependentType = DependencyDetectionLoopTool.ResolveDependencies(typeof(T));
            var orderedTypes = DependencyDetectionLoopTool.Raw.OrderBy(type => type.Dependecies.Count).ToList();

            orderedTypes.RemoveAll(type => ignoredTypes.Any(type1 => type1 == type.Type));
            var zeroInstancesTypes = numberOfInstances?.Where(pair => pair.Value <= 0).Select(pair => pair.Key);

            orderedTypes.RemoveAll(type => zeroInstancesTypes.Any(type1 => type1 == type.Type));
            if (orderedTypes.FirstOrDefault(type => type.Type == dependentType.Type) == null) orderedTypes.Add(dependentType);


            foreach (var orderedType in orderedTypes)
            {
                var method = typeof(SimInstanceManager).GetMethod("GenerateInstancesWithRules", new[] { typeof(int), typeof(bool) });
                var generic = method.MakeGenericMethod(orderedType.Type);
                if (numberOfInstances.ContainsKey(orderedType.Type))
                {
                    SimContainer.Container.Add(orderedType.Type, (List<object>)generic.Invoke(null, new object[] { numberOfInstances[orderedType.Type], true }));
                }
                else
                {
                    SimContainer.Container.Add(orderedType.Type, (List<object>)generic.Invoke(null, new object[] { 1, true }));
                }

            }
            var resultObjects = SimContainer.Container.GetAll(typeof(T));
            var result = new List<T>();
            foreach (var resultObject in resultObjects)
            {
                result.Add((T)resultObject);
            }
            return result;
        }
    }
}