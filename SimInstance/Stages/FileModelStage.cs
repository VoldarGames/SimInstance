using System;
using SimInstance.Profiles.ModelProfiles;
using SimInstanceLab.Managers;

namespace SimInstance.Stages
{
    public class FileModelStage : Stage
    {
        public FileModelStage()
        {
            var simDatabaseInFileProvider =  new SimFileDatabaseProvider() {AbsolutePath = "V:\\SimInstanceDatabase" };
            this
                .UseSeed((int)DateTime.Now.Ticks)
                .UseProfile(new ModelIntClassSimRulesProfile(), forcePrimaryKey: true)
                .UseProfile(new ModelStringClassSimRulesProfile(), forcePrimaryKey: true)
                .UseProvider(simDatabaseInFileProvider);
        }
    }
}