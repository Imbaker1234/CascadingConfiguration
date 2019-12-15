using System;
using System.ComponentModel;
using System.Text;

namespace CascadingConfiguration
{
    public abstract class Config : IConfig
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var property in GetType().GetProperties())
            {
                sb.Append($"{property.ToString().Replace(' ', ':')}={property.GetValue(this)}{Environment.NewLine}");
            }

            return sb.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged
        {
            //TODO DO THE STUFF HERE!!!!
        }
    }
}