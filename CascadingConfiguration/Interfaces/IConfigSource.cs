using System.Collections.Generic;
using System.Reflection;

namespace CascadingConfiguration
{
    public interface IConfigSource<in T> where T : IConfig
    {
        /// <summary>
        /// <para>
        /// Determines the importance of a particular source
        /// and thereby the order in which the source gets
        /// iterated over.
        /// </para>
        /// <para>
        /// In most methods of population the higher this value
        /// is the earlier it gets populated with the notable
        /// exception being CascadingPopulate() which does so
        /// in reverse order to allow values sourced from those
        /// of a higher priority to override one of a lower priority. 
        /// </para>
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// <para>
        /// Determines if the source is present before adding it to the
        /// ConfigProviders list of sources. 
        /// </para>
        /// <para>
        /// Additionally checks to see if the source has a corresponding
        /// value for CCSPriority and, if found, allows the source to
        /// override the hardcoded value.
        /// </para>
        /// </summary>
        /// <returns></returns>
        bool Initialize();

        /// <summary>
        /// This method is unique to each source but the fundamentals of this
        /// method are that once the values are retrieved from the source they
        /// are matched with the property names specified in the IConfig object
        /// before being converted and set.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="unsetProperties"></param>
        /// <returns></returns>
        HashSet<PropertyInfo> PopulateConfig(T config, HashSet<PropertyInfo> unsetProperties);
    }
}