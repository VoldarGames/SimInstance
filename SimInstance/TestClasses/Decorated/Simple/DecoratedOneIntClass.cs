using SimInstanceLab.SimAttributes;

namespace SimInstance.TestClasses.Decorated.Simple
{
    public class DecoratedOneIntClass
    {
        [SimRange(0,10)]
        public int MyInt { get; set; }
    }
}