using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeirachicalConfiguration;
using HeirachicalConfiguration.Interfaces;
using Moq;
using NUnit.Framework;
using Sample;

namespace HierarchicalConfigurationTests
{
    [TestFixture]
    public class ConfigSourceTesting
    {
        [Test]
        public void SourcedConfigWillThrowErrorsWhenInvalidTypeIsSupplied()
        {
            var configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "SourcedConfigWillThrowErrorsWhenInvalidTypeIsSupplied.txt");

            var mockProvider = new Mock<IConfigProvider<SampleConfig>>();

            var sut = new TextFileConfigSource<SampleConfig>(configPath, '=', null, mockProvider.Object, 2);

            //var sut = new SampleTextSource(mockProvider.Object, 2, configPath, null);

            var product = sut.SourceConfig();

            Console.WriteLine(product.ToString());
        }
    }
}
