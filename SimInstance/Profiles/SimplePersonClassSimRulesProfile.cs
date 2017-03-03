using SimInstance.TestClasses.Undecorated.Simple;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstance.Profiles
{
    public class SimplePersonClassSimRulesProfile : AbstractSimRulesProfile<SimplePersonClass>
    {
        public SimplePersonClassSimRulesProfile()
        {
            SimRuleFor(c => c.Age, new SimRangeAttribute(0, 100));
            SimRuleFor(c => c.Name, new SimRegexAttribute("[A-Z][a-z]{5}") );
            SimRuleFor(c => c.SurName, new SimRegexAttribute("[A-Z][a-z]{4}"));
            SimRuleFor(c => c.Street, new SimRegexAttribute("[A-Z][a-z]{5} Street"));
        }


    }
}