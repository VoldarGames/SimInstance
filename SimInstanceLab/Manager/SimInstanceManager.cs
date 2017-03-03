using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using SimInstanceLab.SimAttributes.BaseClass;
using SimInstanceLab.SimAttributes.Handler;
using SimInstanceLab.SimRules;

namespace SimInstanceLab.Manager
{
    public class SimInstanceManager
    {
        /// <summary>
        /// Add dynamically at runtime new SimAttributes to our T Type and creates an instance of this Type.
        ///  It is useful because we can add decorators to our model without putting them in a real scenario and breaking dependencies with this tool.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="simRules"></param>
        /// <returns></returns>
        public T CreateInstanceWithRules<T>(List<SimRule<T>> simRules)
        {
            var type = typeof(T);

            var assemblyName = new System.Reflection.AssemblyName("SimInstanceLab");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = moduleBuilder.DefineType($"SimClass_{type.Name}", System.Reflection.TypeAttributes.Public,typeof(T));

            
            foreach (var propertyInfo in type.GetRuntimeProperties().Where(info => info.DeclaringType != null && info.DeclaringType.IsPublic))
            {
                var propertyBuilder = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.HasDefault, CallingConventions.Any, propertyInfo.PropertyType,null);
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
                                new Type[] {propertyInfo.PropertyType});

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
        public List<T> GenerateInstances<T>(int count) where T : new()
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
        /// <param name="count">The number of instances that you want to build randomly according to SimRules.</param>
        /// <param name="simRules">The SimRules affecting this new instance.</param>
        /// <returns></returns>
        public List<T> GenerateInstancesWithRules<T>(int count, List<SimRule<T>> simRules)
        {
            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                var newEntity = CreateInstanceWithRules<T>(simRules);
                HandleNewEntitySimAttributes(ref newEntity,simRules);
                MapToOriginalEntity(ref newEntity);
                result.Add(newEntity);
            }
            return result;
        }

        private void HandleNewEntitySimAttributes<T>(ref T newEntity, List<SimRule<T>> simRules = null)
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
                        property.SetValue(newEntity,newChildEntity);
                    }
                    else
                    {
                        //TODO: Create new Type
                        //Create new Type
                        //Activator.CreateInstance of new Type with SimAttributes
                        //HandleNewEntitySimAttributes(ref newChildEntity);
                        //property.SetValue(newEntity, newChildEntity);
                    }
                }

            }
           
        }

        private void MapToOriginalEntity<T>(ref T newEntity)
        {
            var newEntityType = newEntity.GetType();
            var newEntityPropertiesLab = newEntityType.GetRuntimeProperties().Where(info => info.Module.ScopeName == "SimInstanceLab").ToList();
            var newEntityProperties = newEntityType.GetRuntimeProperties().Except(newEntityPropertiesLab).ToList();
            for (var i = 0; i < newEntityProperties.Count; i++)
            {
                newEntityProperties[i].SetValue(newEntity,newEntityPropertiesLab[i].GetValue(newEntity));
            }

        }

        /// <summary>
        /// Gets all SimAttributes in the Property 'property' in our newEntity
        /// </summary>
        /// <typeparam name="T">Type of newEntity instance.</typeparam>
        /// <param name="newEntity">A new instance with SimAttributes</param>
        /// <param name="property">A property in newEntity.</param>
        private void ApplySimAttributes<T>(ref T newEntity, PropertyInfo property)
        {

            foreach (var simAttribute in property.GetCustomAttributes<SimAttribute>())
            {
                ApplySimAttribute<T>(property,(SimAttribute)simAttribute, ref newEntity);
                
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of newEntity instance.</typeparam>
        /// <param name="property">A property in newEntity.</param>
        /// <param name="simAttribute">The SimAttribute that we need to handle</param>
        /// <param name="newEntity">A new instance with SimAttributes</param>
        private void ApplySimAttribute<T>(PropertyInfo property, SimAttribute simAttribute, ref T newEntity)
        {
            SimAttributesHandler<T>.ActionDictionary[simAttribute.GetType()].Invoke(property,newEntity);
            
        }

        
    }
}