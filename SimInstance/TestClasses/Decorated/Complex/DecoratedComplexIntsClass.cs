using SimInstance.TestClasses.Decorated.Simple;
using SimInstanceLab.SimAttributes;

namespace SimInstance.TestClasses.Decorated.Complex
{
    public class DecoratedComplexIntsClass
    {
        [SimRange(20,30)]
        public int MyInt { get; set; }

        public DecoratedOneIntClass MyIntClass {get; set;}
    }
}