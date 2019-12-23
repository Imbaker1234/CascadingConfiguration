using System;
using System.Collections.Generic;
using System.Reflection;
using CascadingConfiguration.Extensions;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    /// <summary>
    /// <para>
    /// Specifies a configuration held in a database with the following specifications.
    /// </para>
    ///
    /// 
    /// The values are all held in the same table, held in the same column,
    /// identified by matching property name in the same ID column specified
    /// in the WHERE clause.
    /// </para>
    /// </summary>
    /// <typeparam name="ConfigProvider<T>"></typeparam>
    public class KvpDbConfigSource<T> : DbConfigSource<T> where T : IConfig, new()
    {
        public string ValueColumn { get; set; }

        /// <summary>
        /// Using a property first approach, each property seeks out a matching
        /// key column entry for its corresponding value column entry. If not
        /// found it skips the value.
        /// </summary>
        /// <returns></returns>
        public override HashSet<PropertyInfo> PopulateConfig(IConfig config, HashSet<PropertyInfo> unsetProperties)
        {
            if (unsetProperties is null || unsetProperties.Count is 0)
                unsetProperties = new HashSet<PropertyInfo>(config.GetType().GetProperties());

            foreach (var property in unsetProperties)
            {
                var value = DbOperator.SelectScalar(
                    selectThis: ValueColumn,
                    fromTable: Table,
                    whereThis: IdColumn,
                    equalsThat: property.Name).ToString();

                config.SetProperty(property, value);

                unsetProperties.Remove(property);
            }

            return unsetProperties;
        }

        /// <summary>
        /// <para>
        /// This method checks for the CCSPriority value. If
        /// found the value will override the preset priority
        /// hardcoded into this application.
        /// </para>
        /// <para>
        /// If the database is unavailable the query will throw
        /// an exception. Forcing this to return false and causing
        /// the ConfigProvider to remove it from its list of sources.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public override bool Initialize()
        {
            try
            {
                var ccsPriority = int.Parse(DbOperator.SelectScalar(
                    selectThis: ValueColumn,
                    fromTable: Table,
                    whereThis: IdColumn,
                    equalsThat: "CCSPriority").ToString());

                if (ccsPriority != 0)
                {
                    Priority = ccsPriority;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public KvpDbConfigSource(string connectionString, string table, int priority) : base(connectionString, table,
            priority)
        {
        }
    }
}