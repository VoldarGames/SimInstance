using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimInstance
{
    public class SimInstanceManager
    {
        public List<T> GenerateInstances<T>(int count) where T : new()
        {
            var result = new List<T>();

            for (var i = 0; i < count; i++)
            {
                var newEntity = new T();
                foreach (var property in newEntity.GetType().GetRuntimeProperties())
                {
                    if (property.PropertyType.IsPrimitive)
                    {
                        ApplySimAttributes<T>(ref newEntity,property);
                    }
                    else
                    {
                        //Complex? //Other?
                    }
                    result.Add(newEntity);
                }
            }
            return result;

        }

        private void ApplySimAttributes<T>(ref T newEntity, PropertyInfo property)
        {
            foreach (var simAttribute in property.GetCustomAttributes(typeof(SimAttribute)))
            {
                ApplySimAttribute<T>(property,simAttribute, ref newEntity);
                
            }
            
        }

        private void ApplySimAttribute<T>(PropertyInfo property, Attribute simAttribute, ref T newEntity)
        {
            SimAttributesHandler<T>.ActionDictionary[simAttribute.GetType()].Invoke(property,newEntity);
            
        }

        
    }
}