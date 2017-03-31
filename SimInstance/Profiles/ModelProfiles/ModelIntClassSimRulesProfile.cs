using System.Runtime.InteropServices;
using SimInstance.TestClasses.Undecorated.Complex.Model;
using SimInstanceLab.SimAttributes;
using SimInstanceLab.SimRules;
using SimInstanceLab.SimRules.AbstractProfile;

namespace SimInstance.Profiles.ModelProfiles
{
    public class ModelIntClassSimRulesProfile : AbstractSimRulesProfile<ModelIntClass>
    {
        public ModelIntClassSimRulesProfile()
        {
           SimRuleFor(c => c.Id,new SimPrimaryKeyAttribute());
           SimRuleFor(c => c.ModelStringId, new SimForeignKeyAttribute(typeof(ModelStringClass)));
           SimRuleFor(c => c.MyInt, new SimRangeAttribute(0,100));
           SimRuleFor(c => c.MyStringClass, new SimNavigationAttribute(nameof(ModelIntClass.ModelStringId)));
        }


    }
}
