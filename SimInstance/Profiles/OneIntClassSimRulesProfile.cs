using SimInstance.TestClasses.Undecorated.Simple;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstance.Profiles
{
    public class OneIntClassSimRulesProfile : AbstractSimRulesProfile<OneIntClass>
    {
        public OneIntClassSimRulesProfile()
        {
            SimRuleFor(c => c.MyInt, new SimRangeAttribute(0, 100));
        }

        
    }
}