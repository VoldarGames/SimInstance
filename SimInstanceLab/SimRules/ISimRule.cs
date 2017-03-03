using System;
using SimInstanceLab.SimAttributes.BaseClass;

namespace SimInstanceLab.SimRules
{
    public interface ISimRule
    {
        Type EntityType { get; set; }
        string PropertyName { get; set; }
        Type PropertyType { get; set; }
        SimAttribute SimAttribute { get; set; }
    }
}