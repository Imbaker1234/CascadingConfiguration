using System.Collections.Generic;
using System.Reflection;

namespace CascadingConfiguration
{
    public interface IConfigProvider<T> where T : IConfig
    {
        /// <summary>
        /// The values that this object is responsible for supplying are held
        /// here.
        /// </summary>
        T Config { get; set; }

        /// <summary>
        /// A list of sources which are iterated over in various orders and by
        /// various means defined by priority, population method, and the sources
        /// themselves to garner the values for the IConfig object.
        /// </summary>
        List<IConfigSource<T>> Sources { get; set; }

        /// <summary>
        /// Determines whether or not all of the values specified in the IConfig
        /// object are absolutely essential or not.
        /// </summary>
        bool AllowIncompleteConfiguration { get; set; }
    }
}