namespace SimInstance
{
    public class SimpleClassSimRulesProfile : AbstractSimRulesProfile<SimplePersonClass>
    {
        public SimpleClassSimRulesProfile()
        {
            RuleFor(c => c.Age, new SimRangeAttribute(0, 100))
                .RuleFor(c => c.Name, new SimRegexAttribute("[A-Z][a-z]{5}") )
                .RuleFor(c => c.SurName, new SimRegexAttribute("[A-Z][a-z]{4}"))
                .RuleFor(c => c.Street, new SimRegexAttribute("[A-Z][a-z]{5} Street"))
                ;
        }


    }
}