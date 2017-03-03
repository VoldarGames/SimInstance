using SimInstance.TestClasses.Undecorated.Complex;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstance.Profiles
{
    public class ComplexIntsClassSimRulesProfile : AbstractSimRulesProfile<ComplexIntsClass>
    {
        public ComplexIntsClassSimRulesProfile()
        {
            RuleFor(c => c.MyInt, new SimRangeAttribute(20, 30)).
                RuleFor(c => c.MyIntClass.MyInt, new SimRangeAttribute(0,100))
                ;
        }


    }
}