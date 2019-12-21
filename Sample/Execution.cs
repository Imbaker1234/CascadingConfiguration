using System.Collections.Generic;
using System.Configuration;
using CascadingConfiguration;
using CascadingConfiguration.Classes.ConfigSource;
using CascadingConfiguration.Classes.ConfigSource.Database;

namespace Sample
{
    public class Execution
    {
        public void Setup()
        {
            //This might be dropped on the desktop to provide on the fly configuration overrides. People would likely just modify the app config
            //but in the event they don't want to alter it or simply wish to override a property or two this would allow for that.
            var txtSource = 
                new TextFileConfigSource<SampleConfig>(
                    @"C:\Users\Admin\Desktop\SampleText.ccs", 
                    ',', 1);

            //This is where we would typically get our configuration from.
            var dbSource = 
                new KvpDbConfigSource<SampleConfig>(
                    ConfigurationManager.
                    ConnectionStrings["UAS Server"].ToString(),
                "TestCenterConfiguration", 2);

            //Of course if the DB is unavailable we might source our configuration from here.
            var appConfig = new AppConfigSource<SampleConfig>(3);

            var cfgProvider = 
                new ConfigProvider<SampleConfig>(txtSource, dbSource, appConfig);

            cfgProvider.AllowIncompleteConfiguration = false; //Let's say for this particular implementation all values are absolutely essential and we should
            //throw an exception if they fail to populate over the course of iterating across our ConfigSources.

            cfgProvider.CascadingPopulate();

            var KingKrab = cfgProvider.Config;
        }
    }
}