using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace CascadingConfiguration
{
    /// <summary>
    /// <para>
    /// Specifies a configuration held in a database with the following specifications.
    /// </para>
    ///
    /// <para>
    /// The values are all held in the same table.
    /// </para>
    /// <para>
    /// The values are all held in the same column.
    /// </para>
    /// <para>
    /// The values are all identified by the same ID column specified in the WHERE clause.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class KVPDatabaseConfigSource<T> : ConfigSource<T> where T : IConfig, new()
    {
        public string ConnectionString { get; set; }
        public string ConfigTable { get; set; }
        public string ValueColumn { get; set; }
        public string IdColumn { get; set; }

        // TODO Consider adding Set vs Unset Properties at the class level so that they can be passed from one source to another.
        // TODO Create a single record DB Config Source.
        // TODO Create a specific DB config source that runs on "PropertyName=SQL Command"
        // TODO Allow for multi-set functions iterating over a recordsets IDs and setting properties based off of that.
        // TODO Extract the base/common elements of these DBConfigSources to an abstract class.
        /// <summary>
        /// Sets the properties for the IConfig held in this class. If the 
        /// </summary>
        /// <param name="unsetProperties"></param>
        /// <returns></returns>
        public override List<PropertyInfo> SourceConfig(List<PropertyInfo> unsetProperties = null)
        {
            if (unsetProperties == null) unsetProperties = Config.GetType().GetProperties().ToList();

            using (var cnn = new SqlConnection(ConnectionString))
            {
                foreach (var property in Config.GetType().GetProperties())
                {
                    try
                    {
                        string key = property.Name;

                        var sql = $"SELECT {ValueColumn}" +
                                  $"FROM {ConfigTable}" +
                                  $"WHERE {IdColumn} = '{key}'";

                        var command = new SqlCommand(sql, cnn);

                        var value = command.ExecuteScalar().ToString();

                        Config.SetProperty(property, value);
                    }
                    catch (Exception e)
                    {
                        
                    }
                }

                return default(T); //TODO remove this and put proper return
            }
        }

//        private T QuerySingleValue(Property)
//        {
//        }
    }
}