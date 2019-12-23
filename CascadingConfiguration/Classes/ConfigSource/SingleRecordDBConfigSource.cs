using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CascadingConfiguration.Extensions;

namespace CascadingConfiguration.Classes.ConfigSource.Database
{
    public class SingleRecordDbConfigSource<T> : DbConfigSource<T> where T : IConfig, new()
    {
        public string IdValue { get; set; }

        /// <summary>
        /// Generates configuration values pulled from a single record where the column and property
        /// values are identical.
        /// </summary>
        /// <returns></returns>
        public override HashSet<PropertyInfo> PopulateConfig(IConfig config, HashSet<PropertyInfo> unsetProperties)
        {
            if (unsetProperties is null || unsetProperties.Count is 0)
                unsetProperties = new HashSet<PropertyInfo>(config.GetType().GetProperties());

            string selectValue = "SELECT TOP 1 *"; //What we want.

            var result = DbOperator.Select(
                selectThis: selectValue,
                fromTable: Table,
                whereThis: IdColumn,
                equalsThat: IdValue).First(); //Retrieves the record.

                foreach (var property in unsetProperties)
                {
                    if(!result.ContainsKey(property.Name)) continue; //Checks if we have a matching key.

                    var value = result[property.Name].ToString();

                    config.SetProperty(property, value);

                    unsetProperties.Remove(property);
                }

                return unsetProperties;
        }

        public override bool Initialize()
        {
            try
            {
                var ccsPriority = int.Parse(DbOperator.SelectScalar(
                    selectThis: "Top 1 CCSPriority",
                    fromTable: Table,
                    whereThis: IdColumn,
                    equalsThat: IdValue).ToString());

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


        public SingleRecordDbConfigSource(string connectionString, string table, int priority) : base(connectionString, table, priority)
        {
            Priority = priority;
        }
    }
}