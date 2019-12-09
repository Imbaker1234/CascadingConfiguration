using System;
using System.Text;

namespace HeirachicalConfiguration
{
    public class Config : IConfig
    {
        public bool FullyConfigured { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var property in GetType().GetProperties())
            {
                sb.Append($"{property.ToString().Replace(' ', ':')}={property.GetValue(this)}{Environment.NewLine}");
            }

            return sb.ToString();
        }
    }
}