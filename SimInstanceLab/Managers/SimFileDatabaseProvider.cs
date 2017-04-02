using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SimInstanceLab.Managers.Helpers;
using SimInstanceLab.SimRules.PrimaryKeyMap;

namespace SimInstanceLab.Managers
{
    public class SimFileDatabaseProvider : ISimProvider
    {
        public string AbsolutePath = "";

        private static void ManagePrimaryKeys(List<object> entities)
        {
            if (!entities.Any()) return;

            var primaryKeyPropertyName = SimPrimaryKeyMap.GetPrimaryKeyPropertyName(entities.FirstOrDefault()?.GetType());
            var primaryKeyProperty = entities.FirstOrDefault()?.GetType().GetProperty(primaryKeyPropertyName);

            var allPrimaryKeysIntegers = entities.AsParallel().Select(entity => (int) entity.GetType().GetProperty(primaryKeyPropertyName).GetValue(entity)).ToList();
            var maximumPrimaryKey = allPrimaryKeysIntegers.OrderByDescending(i => i).FirstOrDefault();
            RandomSeedHelper.SetCounter((uint) (maximumPrimaryKey+1));

            foreach (var entity in entities)
            {
               
                if((int)primaryKeyProperty.GetValue(entity) <= 0 ) primaryKeyProperty.SetValue(entity, RandomSeedHelper.GetNextAutoIncrementalNumber());

            }
        }

        public void Add(Type type, List<object> entities)
        {
            if (string.IsNullOrEmpty(AbsolutePath)) throw new SimFileDatabaseAbsolutePathNotDefinedException();
            if (!ContainsKey(type))
            {
                ManagePrimaryKeys(entities);
                using (var streamWriter = File.CreateText($"{AbsolutePath}\\{type.Name}"))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(entities));
                }
            }
            else
            {
                
                var allStoredObjects = GetAll(type);
                allStoredObjects.AddRange(entities);
                ManagePrimaryKeys(allStoredObjects);
                using (var streamWriter = File.CreateText($"{AbsolutePath}\\{type.Name}"))
                {
                    streamWriter.Write(JsonConvert.SerializeObject(allStoredObjects));
                }
            }
        }

        public object GetById(Type type, int id)
        {
            if (string.IsNullOrEmpty(AbsolutePath)) throw new SimFileDatabaseAbsolutePathNotDefinedException();

            var containerPropertyType = type.GetProperty(SimPrimaryKeyMap.GetPrimaryKeyPropertyName(type));

            var allStoredObjects = GetAll(type);

            return allStoredObjects.AsParallel().Single(o => (int) containerPropertyType.GetValue(o) == id);
        }

        public bool ContainsKey(Type propertyType)
        {
            return File.Exists($"{AbsolutePath}\\{propertyType.Name}");
        }

        public List<object> GetAll(Type type)
        {
            if (string.IsNullOrEmpty(AbsolutePath)) throw new SimFileDatabaseAbsolutePathNotDefinedException();

            var listType = typeof(List<>);
            var genericTypeList = listType.MakeGenericType(type);

            var deserializeMethod = typeof(JsonConvert).GetMethods().FirstOrDefault(m => m.Name == "DeserializeObject" && m.IsGenericMethod);
            var generic = deserializeMethod.MakeGenericMethod(genericTypeList);

            var result =  generic.Invoke(null, new object[] {File.ReadAllText($"{AbsolutePath}\\{type.Name}")}) as IEnumerable;

            return result.Cast<object>().ToList(); ;


            
            //return JsonConvert.DeserializeObject<List<object>>(File.ReadAllText($"{AbsolutePath}\\{type.Name}"));
        }

        public int GetCount(Type type)
        {
            if (string.IsNullOrEmpty(AbsolutePath)) throw new SimFileDatabaseAbsolutePathNotDefinedException();
            return  GetAll(type).Count;
        }
    }

    public class SimFileDatabaseAbsolutePathNotDefinedException : Exception
    {
        public SimFileDatabaseAbsolutePathNotDefinedException() : base("You need to configure the AbsolutePath in your SimFileDatabaseProvider"){}
    }
}