using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using SimInstanceLab.Managers.Helpers;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimAttributes.Handler;
using SimInstanceLab.SimRules;
using SimInstanceLab.SimRules.NavigationMap;

namespace SimInstanceLab.Managers
{
    public static class SimInstanceManager
    {

        public static List<Type> LoopDetectionList = new List<Type>();
        /// <summary>
        /// Add dynamically at runtime new SimAttributes to our T Type and creates an instance of this Type.
        ///  It is useful because we can add decorators to our model without putting them in a real scenario and breaking dependencies with this tool.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="simRules"></param>
        /// <returns></returns>
        public static T CreateInstanceWithRules<T>(List<ISimRule> simRules)
        {
            var type = typeof(T);

            var assemblyName = new System.Reflection.AssemblyName("SimInstanceLab");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = moduleBuilder.DefineType($"SimClass_{type.Name}", System.Reflection.TypeAttributes.Public, typeof(T));


            foreach (var propertyInfo in type.GetRuntimeProperties().Where(info => info.DeclaringType != null && info.DeclaringType.IsPublic))
            {
                var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.HasDefault, CallingConventions.Any, propertyInfo.PropertyType, null);
                foreach (var simRule in simRules)
                {
                    if (propertyInfo.Name == simRule.PropertyName)
                    {
                        var attributeCtorParams = simRule.SimAttribute.GetParameterTypes();
                        var attributeCtorInfo = simRule.SimAttribute.GetType().GetConstructor(attributeCtorParams);
                        var attributeBuilder = new CustomAttributeBuilder(attributeCtorInfo,
                            simRule.SimAttribute.GetParameterValues());
                        propertyBuilder.SetCustomAttribute(attributeBuilder);


                        FieldBuilder fieldBuilder = typeBuilder.DefineField($"_{propertyInfo.Name}",
                            typeof(string),
                            FieldAttributes.Private);

                        const MethodAttributes getSetAttributes =
                            MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
                        // Define the "get" accessor method for CustomerName.
                        MethodBuilder methodBuilderGet =
                            typeBuilder.DefineMethod($"get_{propertyInfo.Name}",
                                getSetAttributes,
                                propertyInfo.PropertyType,
                                Type.EmptyTypes);
                        ILGenerator getIL = methodBuilderGet.GetILGenerator();
                        getIL.Emit(OpCodes.Ldarg_0);
                        getIL.Emit(OpCodes.Ldfld, fieldBuilder);
                        getIL.Emit(OpCodes.Ret);
                        // Define the "set" accessor method for CustomerName.
                        MethodBuilder methodBuilderSet =
                            typeBuilder.DefineMethod($"set_{propertyInfo.Name}",
                                getSetAttributes,
                                null,
                                new Type[] { propertyInfo.PropertyType });

                        ILGenerator setIL = methodBuilderSet.GetILGenerator();
                        setIL.Emit(OpCodes.Ldarg_0);
                        setIL.Emit(OpCodes.Ldarg_1);
                        setIL.Emit(OpCodes.Stfld, fieldBuilder);
                        setIL.Emit(OpCodes.Ret);

                        // Last, we must map the two methods created above to our PropertyBuilder to 
                        // their corresponding behaviors, "get" and "set" respectively. 

                        propertyBuilder.SetGetMethod(methodBuilderGet);
                        propertyBuilder.SetSetMethod(methodBuilderSet);
                    }
                }
            }

            var newType = typeBuilder.CreateType();
            var instance = Activator.CreateInstance(newType);
            return (T)instance;
        }


        /// <summary>
        /// Generates 'count' number of instances that have decorator SimAttributes in their public properties
        /// </summary>
        /// <typeparam name="T">New instances Type</typeparam>
        /// <param name="count">The number of instances that you want to build randomly according to SimAttributes.</param>
        /// <returns></returns>
        public static List<T> GenerateInstances<T>(int count) where T : new()
        {
            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                var newEntity = new T();
                HandleNewEntitySimAttributes(ref newEntity);
                result.Add(newEntity);
            }
            return result;

        }

        /// <summary>
        /// Generates 'count' number of instances according to the simRules
        /// </summary>
        /// <typeparam name="T">New instances Type</typeparam>
        /// <param name="count">The number of instances that you want to build randomly according to SimRules in ProfileManager.</param>
        /// <returns></returns>
        public static List<T> GenerateInstancesWithRules<T>(int count) where T : new()
        {
            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                var newEntity = new T();

                HandleNewEntitySimRulesProfile<T>(ref newEntity);
                LoopDetectionList.Clear();
                GenerateComplexChildren<T>(ref newEntity);

                result.Add(newEntity);
            }
            return result;
        }

        public static List<object> GenerateInstancesWithRules<T>(int count, bool withContainerInstances) where T : new()
        {
            var result = new List<object>();
            
            for (var i = 0; i < count; i++)
            {
                var newEntity = new T();

                HandleNewEntitySimRulesProfile<T>(ref newEntity);
                LoopDetectionList.Clear();
                GenerateComplexChildren<T>(ref newEntity, withContainerInstances);

                result.Add(newEntity);
            }
            return result;
        }


        private static void GenerateComplexChildren<T>(ref T newEntity, bool withContainerInstances = false)
        {
            var newEntityType = newEntity.GetType();
            if (LoopDetectionList.Count(t => t == newEntityType) > 1) return;


            var runtimeProperties = newEntity.GetType().GetRuntimeProperties();
            foreach (var property in runtimeProperties)
            {

                try
                {

                    //TODO ARRAYS, COLLECTIONS
                    if (!PrimitiveOrClassHelper.IsPrimitive(property.PropertyType) && !property.PropertyType.IsArray && !property.PropertyType.IsAssignableFrom(typeof(IEnumerable)) && !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string) && !property.PropertyType.IsInterface)
                    {

                        if (property.PropertyType.GetInterfaces().Contains(typeof(IEnumerable))) continue;
                        if (property.DeclaringType.GetInterfaces().Contains(typeof(IEnumerable))) continue;
                        if (property.SetMethod == null) continue;
                        object newChildEntity;
                        var generatedFromExistent = false;
                        if (withContainerInstances && SimContainer.Container.ContainsKey(property.PropertyType))
                        {

                            if (!SimContainer.MemoryContainer.ContainsKey(property.PropertyType))
                            {
                                SimContainer.MemoryContainer.Add(property.PropertyType, SimContainer.Container.GetAll(property.PropertyType));
                            }
                            if (SimContainer.MemoryContainer.GetCount(property.PropertyType) == 0) throw new SimCantGenerateException($"{property.PropertyType} doesn't have any instance, you need to generate at least one.");

                            if (SimNavigationMap.Exist(newEntityType, property.Name))
                            {
                                var foreignKeyPropertyName = SimNavigationMap.GetNavigationForeignKeyPropertyName(newEntityType, property.Name);
                                newChildEntity = SimContainer.MemoryContainer.GetById(property.PropertyType,
                                    (int) newEntity.GetType().GetProperty(foreignKeyPropertyName).GetValue(newEntity));
                            }
                            else
                            {
                                var randomIndex = RandomSeedHelper.Random.Next(0,
                                    SimContainer.Container.GetCount(property.PropertyType) - 1);
                                var existingEntityList = SimContainer.MemoryContainer.GetAll(property.PropertyType);
                                newChildEntity = existingEntityList[randomIndex];
                            }

                            generatedFromExistent = true;


                        }
                        else
                        {
                            newChildEntity = Activator.CreateInstance(property.PropertyType);

                            if (newChildEntity == null) continue;
                            if (SimRulesProfileManager.IgnoreTypeList.Contains(property.PropertyType)) continue;
                            if (SimRulesProfileManager.ProfilesDictionary.ContainsKey(newEntity.GetType()) &&
                                SimRulesProfileManager.ProfilesDictionary[newEntity.GetType()]
                                    .SimRules.Any(
                                        c =>
                                            c.SimAttribute.GetType() == typeof(SimIgnoreAttribute) &&
                                            c.PropertyName == property.Name))
                            {
                                continue;
                            }

                            HandleNewEntitySimRulesProfile(ref newChildEntity);
                        }

                        if (!generatedFromExistent)
                        {
                            LoopDetectionList.Add(newChildEntity.GetType());
                            GenerateComplexChildren(ref newChildEntity);
                            LoopDetectionList.Remove(newChildEntity.GetType());
                        }

                        property.SetValue(newEntity, newChildEntity);
                    }
                }
                catch (Exception ex)
                {
                    throw new GenerateComplexChildrenException($"{property.Name} on {property.DeclaringType.FullName} failed to create instance. error {ex.Message} ");
                }
            }
        }

        private static void HandleNewEntitySimRulesProfile<T>(ref T newEntity)
        {
#pragma warning disable S2955 // Generic parameters not constrained to reference types should not be compared to "null"
            if (newEntity == null) return;
#pragma warning restore S2955 // Generic parameters not constrained to reference types should not be compared to "null"
            //todo gestionar por profiler new intsnce ???
            if (!SimRulesProfileManager.ProfilesDictionary.ContainsKey(newEntity.GetType())) return;
            var simRulesOfT = SimRulesProfileManager.ProfilesDictionary[newEntity.GetType()].SimRules;

            foreach (var simRule in simRulesOfT)
            {
                SimAttributesHandler<T>.ActionDictionary[simRule.SimAttribute.GetType()].Invoke(newEntity.GetType().GetProperty(simRule.PropertyName), newEntity, simRule.SimAttribute);
            }


        }

        /// <summary>
        /// Generates 'count' number of instances in Lab according to the simRules
        /// </summary>
        /// <typeparam name="T">New instances Type</typeparam>
        /// <param name="count">The number of instances that you want to build randomly according to SimRules.</param>
        /// <param name="simRules">The SimRules affecting this new instance.</param>
        /// <returns></returns>
        public static List<T> GenerateInstancesWithRulesInLab<T>(int count)
        {
            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                var newEntity = CreateInstanceWithRules<T>(SimRulesProfileManager.ProfilesDictionary[typeof(T)].SimRules);
                HandleNewEntitySimAttributes(ref newEntity, SimRulesProfileManager.ProfilesDictionary[typeof(T)].SimRules);
                MapToOriginalEntity(ref newEntity);
                result.Add(newEntity);
            }
            return result;
        }

        private static void HandleNewEntitySimAttributes<T>(ref T newEntity, List<ISimRule> simRules = null)
        {
            var runtimeProperties = newEntity.GetType().GetRuntimeProperties();
            foreach (var property in runtimeProperties)
            {
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    ApplySimAttributes<T>(ref newEntity, property);
                }
                else //Complex Type
                {
                    if (simRules == null)
                    {
                        var newChildEntity = Activator.CreateInstance(property.PropertyType);
                        HandleNewEntitySimAttributes(ref newChildEntity);
                        property.SetValue(newEntity, newChildEntity);
                    }
                    else
                    {
                        var newChildEntity = CreateInstanceWithRules<T>(SimRulesProfileManager.ProfilesDictionary[property.PropertyType].SimRules);
                        HandleNewEntitySimAttributes(ref newChildEntity);
                        property.SetValue(newEntity, newChildEntity);
                    }
                }

            }

        }

        private static void MapToOriginalEntity<T>(ref T newEntity)
        {
            var newEntityType = newEntity.GetType();
            var newEntityPropertiesLab = newEntityType.GetRuntimeProperties().Where(info => info.Module.ScopeName == "SimInstanceLab").ToList();
            var newEntityProperties = newEntityType.GetRuntimeProperties().Except(newEntityPropertiesLab).ToList();
            for (var i = 0; i < newEntityProperties.Count; i++)
            {
                newEntityProperties[i].SetValue(newEntity, newEntityPropertiesLab[i].GetValue(newEntity));
            }

        }

        /// <summary>
        /// Gets all SimAttributes in the Property 'property' in our newEntity
        /// </summary>
        /// <typeparam name="T">Type of newEntity instance.</typeparam>
        /// <param name="newEntity">A new instance with SimAttributes</param>
        /// <param name="property">A property in newEntity.</param>
        private static void ApplySimAttributes<T>(ref T newEntity, PropertyInfo property)
        {

            foreach (var simAttribute in property.GetCustomAttributes<SimAttribute>())
            {
                ApplySimAttribute<T>(property, (SimAttribute)simAttribute, ref newEntity);

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of newEntity instance.</typeparam>
        /// <param name="property">A property in newEntity.</param>
        /// <param name="simAttribute">The SimAttribute that we need to handle</param>
        /// <param name="newEntity">A new instance with SimAttributes</param>
        private static void ApplySimAttribute<T>(PropertyInfo property, SimAttribute simAttribute, ref T newEntity)
        {
            SimAttributesHandler<T>.ActionDictionary[simAttribute.GetType()].Invoke(property, newEntity, simAttribute);

        }



    }

    internal class GenerateComplexChildrenException : Exception
    {
        public GenerateComplexChildrenException(string s) : base(s) { }
    }
}