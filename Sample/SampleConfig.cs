using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CascadingConfiguration;
using CascadingConfiguration.Annotations;

namespace Sample
{
    public class SampleConfig : IConfig
    {
        public bool? TwentyPercentDiscount = false;

        public byte? outOfCrime = null;

        public char? isFor = null;

        public short? babyNumber = null;
        
        public string customerName = null;

        public DateTime? dateVal = DateTime.Now;
    }
}