using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimInstance
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void GenerationGoodCountTest()
        {
            SimInstanceManager manager = new SimInstanceManager();

            var target = manager.GenerateInstances<OneIntClass>(3);

            Assert.IsNotNull(target);
            Assert.AreEqual(target.Count,3);

        }
    }

    public class OneIntClass
    {
        [SimRange(0,10)]
        public int MyInt { get; set; }
    }
}
