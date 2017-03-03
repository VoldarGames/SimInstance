using SimInstance.TestClasses.Undecorated.Simple;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstance.Profiles
{
    public class SimplePersonClassSimRulesProfile : AbstractSimRulesProfile<SimplePersonClass>
    {
        public SimplePersonClassSimRulesProfile()
        {
            RuleFor(c => c.Age, new SimRangeAttribute(0, 100))
                .RuleFor(c => c.Name, new SimRegexAttribute("[A-Z][a-z]{5}") )
                .RuleFor(c => c.SurName, new SimRegexAttribute("[A-Z][a-z]{4}"))
                .RuleFor(c => c.Street, new SimRegexAttribute("[A-Z][a-z]{5} Street"))
                ;
        }


    }
}