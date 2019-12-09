using System.Collections.Generic;

namespace HeirachicalConfiguration.Interfaces
{
    public interface IConfigProvider<T> where T : IConfig
    {
        /// <summary>
        /// Enumerates over the available IConfigSources for the values
        /// of the object we are trying to retrieve.
        /// </summary>
        /// <returns></returns>
        T GetConfig();

        /// <summary>
        /// The list of available sources, text files, databases,
        /// app configs, etc.
        /// </summary>
        List<IConfigSource<T>> Sources { get; set; }

        /// <summary>
        /// Allows for some of the values of the IConfig to fail
        /// to be retrieved without halting the execution.
        /// </summary>
        bool AllowIncompleteConfiguration { get; set; }

        /// <summary>
        ///<para>
        /// Causes the enumeration of IConfig sources to go from
        /// lowest priority to greatest. Values from higher priority
        /// IConfigSources will overwrite values from lower priority
        /// IConfigSources.
        /// </para>
        /// 
        /// <para></para>
        ///
        /// <para>
        /// This provides additional redundancy while also allowing for
        /// the supply of values from overriding sources. For example a
        /// text file with a very specific name overriding the values
        /// from a database.
        /// </para>
        /// </summary>
        bool AllowMultipleSources { get; set; }
    }
}