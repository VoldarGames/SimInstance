using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Fare;
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

            var oneIntClassSimRulesProfile = new OneIntClassSimRulesProfile();
            
            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now-now).ToString("G")}");
            var target = manager.GenerateInstancesWithRules<OneIntClass>(numberOfInstances, oneIntClassSimRulesProfile.SimRules);


            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count, numberOfInstances);

            foreach (var targetInstance in target)
            {
                Assert.IsTrue(targetInstance.MyInt >= 0 && targetInstance.MyInt <= 100);
            }
        }

        [TestMethod]
        public void GenerationDecoratedSimplePersonTest()
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
        public void GenerationWith2RulesSimplePersonTest()
        {
            const int numberOfInstances = 3000;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();

            var simpleClassSimRulesProfile = new SimpleClassSimRulesProfile();

            Debug.WriteLine($"Total elapsed time creating {numberOfInstances} instances: {(DateTime.Now - now).ToString("G")}");
            var target = manager.GenerateInstancesWithRules<SimplePersonClass>(numberOfInstances, simpleClassSimRulesProfile.SimRules);


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
        public void GenerationComplexDecoratedClassTest()
        {
            const int numberOfInstances = 300;
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
        public void GenerationComplexWithRulesClassTest()
        {
            const int numberOfInstances = 300;
            var now = DateTime.Now;

            SimInstanceManager manager = new SimInstanceManager();
            var complexIntsClassSimRulesProfile = new ComplexIntsClassSimRulesProfile();
            var target = manager.GenerateInstancesWithRules<ComplexIntsClass>(numberOfInstances, complexIntsClassSimRulesProfile.SimRules);

            
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
        public void FareDniRegexTest()
        {
            var target = new Xeger("^[0-9]{8}[a-zA-Z]{1}$");
            var randomDni = target.Generate();
        }

        [TestMethod]
        public void FareTest()
        {
            var target = new Xeger("^[a-z]{10}$");
            var randomPrefix = target.Generate();
        }

        [TestMethod]
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
