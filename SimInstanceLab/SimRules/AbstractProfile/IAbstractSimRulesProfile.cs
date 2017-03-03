using System.Collections.Generic;

namespace SimInstanceLab.SimRules.AbstractProfile
{
    public interface IAbstractSimRulesProfile
    {
        List<ISimRule> SimRules { get; set; }
    }
}