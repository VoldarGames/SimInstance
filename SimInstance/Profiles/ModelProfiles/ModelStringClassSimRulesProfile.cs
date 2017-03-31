using SimInstance.TestClasses.Undecorated.Complex.Model;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstance.Profiles.ModelProfiles
{
    public class ModelStringClassSimRulesProfile : AbstractSimRulesProfile<ModelStringClass>
    {
        public ModelStringClassSimRulesProfile()
        {
            SimRuleFor(c => c.Id, new SimPrimaryKeyAttribute());
            SimRuleFor(c => c.MyString,new SimRegexAttribute("SIM STRING: [a-zA-Z]{10}"));
        }
    }
}