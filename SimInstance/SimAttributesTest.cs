using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimInstance.Stages;
using SimInstance.TestClasses.Undecorated.Complex.Model;
using SimInstanceLab.Managers;

namespace SimInstance
{
    [TestClass]
    public class SimAttributesTest
    {
        [TestMethod]
        public void DatabaseInMemoryTest()
        {
            var instances = new ModelStage()
                .Execute<ModelIntClass>(
                    new Dictionary<Type, int>()
                    {
                        {typeof(ModelIntClass), 10000},
                        {typeof(ModelStringClass), 10000},
                    }
                );
            Assert.AreEqual(10000,instances.Count);
        }

        [TestMethod]
        public void DatabaseInFileTest()
        {
            var instances = new FileModelStage()
                .Execute<ModelIntClass>(
                    new Dictionary<Type, int>()
                    {
                        {typeof(ModelIntClass), 100},
                        {typeof(ModelStringClass), 100},
                    }
                );
            Assert.AreEqual(100, instances.Count);
        }
    }
}