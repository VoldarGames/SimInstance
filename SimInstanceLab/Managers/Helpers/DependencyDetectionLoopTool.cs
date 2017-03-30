using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimInstanceLab.Managers.Helpers
{
    public static class DependencyDetectionLoopTool
    {
        internal static List<Type> LoopDetectionList = new List<Type>();
        public static IList<DependentType> Raw = new List<DependentType>();

        public static DependentType ResolveDependencies(Type type)
        {
            LoopDetectionList.Clear();
            var result = new DependentType(type);
            result.ResolveDependencies();
            return result;
        }



        public class DependentType
        {
            public Type Type;
            public IList<DependentType> Dependecies = new List<DependentType>();

            public DependentType(Type type)
            {
                Type = type;
            }
            public void ResolveDependencies()
            {
                if (LoopDetectionList.Count(t => t == Type) > 1) return;

                foreach (var runtimeProperty in Type.GetRuntimeProperties())
                {

                    if (!PrimitiveOrClassHelper.IsPrimitive(runtimeProperty.PropertyType))
                    {
                        //if (IsAssignableToGenericType(runtimeProperty.PropertyType,typeof(IEnumerable<>))/*&& runtimeProperty.PropertyType.IsAssignableFrom(typeof(IEnumerable))*/)
                        //{
                        //    foreach (var genericTypeArgument in runtimeProperty.PropertyType.GenericTypeArguments)
                        //    {
                        //        if (Dependecies.FirstOrDefault(type => type.Type == genericTypeArgument) == null)
                        //            Dependecies.Add(new DependentType(genericTypeArgument));
                        //    }

                        //}
                        //else 
                        if (!IsAssignableToGenericType(runtimeProperty.PropertyType, typeof(IEnumerable<>)))
                        {
                            var newDependentType = new DependentType(runtimeProperty.PropertyType);
                            if (Dependecies.FirstOrDefault(type => type.Type == runtimeProperty.PropertyType) == null)
                            {
                                Dependecies.Add(newDependentType);
                            }
                            if (Raw.FirstOrDefault(type => type.Type == runtimeProperty.PropertyType) == null)
                            {
                                Raw.Add(newDependentType);
                            }

                        }
                    }
                }
                foreach (var dependentType in Dependecies)
                {
                    LoopDetectionList.Add(Type);
                    dependentType.ResolveDependencies();
                    LoopDetectionList.Remove(Type);
                }
            }

            private static bool IsAssignableToGenericType(Type givenType, Type genericType)
            {
                if (givenType.GetInterfaces()
                    .Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
                {
                    return true;
                }

                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                    return true;

                var baseType = givenType.BaseType;

                return baseType != null && IsAssignableToGenericType(baseType, genericType);
            }
        }

    }
}