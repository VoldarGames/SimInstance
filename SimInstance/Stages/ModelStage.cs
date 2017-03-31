using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimInstance.Profiles.ModelProfiles;
using SimInstanceLab.Managers;

namespace SimInstance.Stages
{
    public class ModelStage : Stage
    {
        public ModelStage()
        {
            this
                .UseSeed((int) DateTime.Now.Ticks)
                .UseProfile(new ModelIntClassSimRulesProfile(),forcePrimaryKey: true)
                .UseProfile(new ModelStringClassSimRulesProfile(),forcePrimaryKey: true)
                .UseProvider(new SimDatabaseInMemoryProvider());
        }
    }
}
