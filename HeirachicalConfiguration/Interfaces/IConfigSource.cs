using System;
using System.Collections.Generic;
using System.Reflection;
using HeirachicalConfiguration.Interfaces;

namespace HeirachicalConfiguration
{
    public interface IConfigSource<T> where T : IConfig

    {
        T Config { get; set; }

        IConfigProvider<T> Provider { get; set; }

        int Priority { get; set; }

        T SourceConfig();
    }
}