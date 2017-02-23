namespace SimInstance
{
    public class OneIntClassSimRulesProfile : AbstractSimRulesProfile<OneIntClass>
    {
        public OneIntClassSimRulesProfile()
        {
            RuleFor(c => c.MyInt, new SimRangeAttribute(0, 100));
        }

        
    }
}