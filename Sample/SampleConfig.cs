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
        private bool? TwentyPercentDiscount = false;

        private byte? outOfCrime = null;

        private char? isFor = null;

        private short? babyNumber = null;
        
        private string customerName = null;

        private DateTime? dateVal = DateTime.Now;
    }
}