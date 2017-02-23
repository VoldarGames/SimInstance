using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimInstance
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void GenerationGoodCountAndValuesTest()
        {
            const int numberOfInstances = 300;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();
            var target = manager.GenerateInstances<DecoratedOneIntClass>(numberOfInstances);

            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");
            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);
            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 0 && targetInstance.MyInt <= 100);
            }
            
            
        }

        [TestMethod]
        public void GenerationWithRulesGoodCountAndValuesTest()
        {
            const int numberOfInstances = 300;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();
            var simRules = new List<SimRule>()
            {
                new SimRule(nameof(OneIntClass.MyInt),typeof(int),new SimRangeAttribute(0,100))
            };
            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now-now).ToString("G")}");
            var target = manager.GenerateInstancesWithRules<OneIntClass>(numberOfInstances, simRules);


            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);
            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 0 && targetInstance.MyInt <= 100);
            }
        }

        [TestMethod]
        public void CanAddAttribute()
        {
            var type = typeof(DecoratedOneIntClass);

            var assemblyName = new System.Reflection.AssemblyName("SimInstanceLab");
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            var typeBuilder = moduleBuilder.DefineType($"SimClass_{type.Name}", System.Reflection.TypeAttributes.Public, type);

            var propertyBuilder = typeBuilder.DefineProperty("MyInt", PropertyAttributes.None, CallingConventions.Any,type.GetProperty("MyInt").PropertyType, null);

            var attrCtorParams = new[] { typeof(int), typeof(int) };
            var attrCtorInfo = typeof(SimRangeAttribute).GetConstructor(attrCtorParams);
            var attrBuilder = new CustomAttributeBuilder(attrCtorInfo, new object[] { 0,10 });
            //typeBuilder.SetCustomAttribute(attrBuilder);
            propertyBuilder.SetCustomAttribute(attrBuilder);

            var newType = typeBuilder.CreateType();
            var instance = (DecoratedOneIntClass)Activator.CreateInstance(newType);

            Assert.IsTrue(instance.MyInt >= 0 && instance.MyInt < 10);
            var attr = (SimRangeAttribute)instance.GetType().GetProperty("MyInt").GetCustomAttributes(typeof(SimRangeAttribute), false).SingleOrDefault();
            Assert.IsNotNull(attr);
            Assert.AreEqual(attr.MinRange, 0);
            Assert.AreEqual(attr.MaxRange, 10);

        }


        
    }
}
