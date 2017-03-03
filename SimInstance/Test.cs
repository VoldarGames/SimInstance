using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Fare;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimInstance.Profiles;
using SimInstance.TestClasses.Decorated.Complex;
using SimInstance.TestClasses.Decorated.Simple;
using SimInstance.TestClasses.Undecorated.Complex;
using SimInstance.TestClasses.Undecorated.Simple;
using SimInstanceLab.Managers;
using SimInstanceLab.SimAttributes;

namespace SimInstance
{

 
    [TestClass]
    public class Test
    {
        [TestMethod]
        [TestCategory("Decorated Class")]
        public void DecoratedSimpleClassSimRangeTest()
        {
            const int numberOfInstances = 3000;
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
        [TestCategory("UnDecorated Class - Lab")]

        public void UnDecoratedSimpleClassSimRangeWithRules_CreatedInLabTest()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();

            SimRulesProfileManager.AddProfile<OneIntClass>(new OneIntClassSimRulesProfile());
            
            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now-now).ToString("G")}");
            var target = manager.GenerateInstancesWithRulesInLab<OneIntClass>(numberOfInstances);


            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);

            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 0 && targetInstance.MyInt <= 100);
            }
        }

        [TestMethod]
        [TestCategory("Decorated Class")]

        public void DecoratedSimplePersonClassSimRegexTest()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();

            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");
            var target = manager.GenerateInstances<DecoratedSimplePersonClass>(numberOfInstances);


            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);
            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.Age >= 0 && targetInstance.Age <= 100);
                Assert.IsNotNull(targetInstance.Name);
                Assert.IsNotNull(targetInstance.SurName);
                Assert.IsNotNull(targetInstance.Street);
                Assert.IsTrue(targetInstance.Street.EndsWith("Street"));
            }
        }

        [TestMethod]
        [TestCategory("UnDecorated Class - Lab")]

        public void UnDecoratedSimplePersonClassSimRegexWithRules_CreatedInLabTest()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();

            SimRulesProfileManager.AddProfile<SimplePersonClass>(new SimplePersonClassSimRulesProfile());

            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");
            var target = manager.GenerateInstancesWithRulesInLab<SimplePersonClass>(numberOfInstances);


            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);
            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.Age >= 0 && targetInstance.Age <= 100);
                Assert.IsNotNull(targetInstance.Name);
                Assert.IsNotNull(targetInstance.SurName);
                Assert.IsNotNull(targetInstance.Street);
                Assert.IsTrue(targetInstance.Street.EndsWith("Street"));
            }
        }

        [TestMethod]
        [TestCategory("Decorated Class")]
        public void DecoratedComplexClassSimRangeTest()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();
            var target = manager.GenerateInstances<DecoratedComplexIntsClass>(numberOfInstances);

            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");
            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);
            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 20 && targetInstance.MyInt <= 30);
                Assert.IsTrue(targetInstance.MyIntClass.MyInt >= 0 && targetInstance.MyIntClass.MyInt <= 100);
            }
            
        }

        [TestMethod]
        [TestCategory("UnDecorated Class - Lab")]
        public void UnDecoratedComplexClassSimRangeWithRules_CreatedInLabTest()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();

            SimRulesProfileManager.AddProfile<ComplexIntsClass>(new ComplexIntsClassSimRulesProfile());
            SimRulesProfileManager.AddProfile<OneIntClass>(new OneIntClassSimRulesProfile());

            
            var target = manager.GenerateInstancesWithRulesInLab<ComplexIntsClass>(numberOfInstances);

            
            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");
            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);
            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 20 && targetInstance.MyInt <= 30);
                Assert.IsTrue(targetInstance.MyIntClass.MyInt >= 0 && targetInstance.MyIntClass.MyInt <= 100);
            }

        }

        [TestMethod]
        [TestCategory("UnDecorated Class - SimRulesProfileManager")]

        public void UnDecoratedSimpleClassSimRangeWithRules_CreatedInSimRulesProfileManager()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();
            SimRulesProfileManager.AddProfile<OneIntClass>(new OneIntClassSimRulesProfile());

            var target = manager.GenerateInstancesWithRules<OneIntClass>(numberOfInstances);


            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");


            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);

            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 0 && targetInstance.MyInt <= 100);
            }
        }

        [TestMethod]
        [TestCategory("UnDecorated Class - SimRulesProfileManager")]

        public void UnDecoratedComplexClassSimRangeWithRules_CreatedInSimRulesProfileManager()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();
            SimRulesProfileManager.AddProfile<OneIntClass>(new OneIntClassSimRulesProfile());
            SimRulesProfileManager.AddProfile<ComplexIntsClass>(new ComplexIntsClassSimRulesProfile());


            var target = manager.GenerateInstancesWithRules<ComplexIntsClass>(numberOfInstances);


            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");


            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);

            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 0 && targetInstance.MyInt <= 100);
            }
        }


        [TestMethod]
        [TestCategory("Laboratory Tests")]
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

        [TestMethod]
        [TestCategory("Fare Nuget Test")]

        public void FareDniRegexTest()
        {
            var target = new Xeger("^[0-9]{8}[a-zA-Z]{1}$");
            var randomDni = target.Generate();
        }

        [TestMethod]
        [TestCategory("Fare Nuget Test")]

        public void FareTest()
        {
            var target = new Xeger("^[a-z]{10}$");
            var randomPrefix = target.Generate();
        }

        [TestMethod]
        [TestCategory("Fare Nuget Test")]
        public void NameGenerationTest()
        {
            var nameList = new List<string>();
            for (int i = 0; i < 2000; i++)
            {
                var generatedName = GenerateName(4, 8, 40);
                if (!nameList.Contains(generatedName) && nameList.FirstOrDefault(s => s.StartsWith(generatedName.Substring(0,4))) == null) nameList.Add(generatedName);
                else i--;
            }
            nameList = nameList.OrderBy(s => s).ToList();

        }

        private string GenerateName(int minNameLegth, int maxNameLength, int connectionLetterProbability, string startsWithRegex = "[A-Z]")
        {
            var lengthName = new Random((int)DateTime.Now.Ticks).Next(minNameLegth, maxNameLength);
           

            for (var i = 1; i < lengthName; i++)
            {
                if (new Random((int) DateTime.Now.Ticks).Next(0, 100) <= connectionLetterProbability)
                {
                    startsWithRegex += "[hlrstxaeiou]";
                    connectionLetterProbability /= 3;
                }
                else
                {
                    if (i%2 == 1) startsWithRegex += "[aeiou]";
                    else startsWithRegex += "[bcdfghjklmpqrstvxyz]";
                }
            }

            var target = new Xeger(startsWithRegex);
            return target.Generate();
        }
    }
}
