using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CascadingConfiguration;

namespace Sample
{
    public class SampleConfig : IConfig
    {
        public bool? SampleBoolProp { get; set; }
        public string SampleStringProp { get; set; }
        public int SampleIntProp { get; set; }
        public double? SampleDoubleProp { get; set; }
        public DateTime? SampleDateTime { get; set; }

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