namespace SimInstance
{
    public class DecoratedComplexIntsClass
    {
        [SimRange(20,30)]
        public int MyInt { get; set; }

        public DecoratedOneIntClass MyIntClass {get; set;}
    }
}