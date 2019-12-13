using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using CascadingConfiguration;
using Moq;
using NUnit.Framework;
using Sample;

namespace HierarchicalConfigurationTests
{
    [TestFixture]
    public class ConfigSourceTesting
    {
//        [Test]
//        public void SourcedConfigWillThrowErrorsWhenInvalidTypeIsSupplied()
//        {
//            var configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
//                "SourcedConfigWillThrowErrorsWhenInvalidTypeIsSupplied.txt");
//
//            var mockProvider = new Mock<IConfigProvider<SampleConfig>>();
//
//            var sut = new TextFileConfigSource<SampleConfig>(configPath, '=', null, mockProvider.Object, 2);
//
//            //var sut = new SampleTextSource(mockProvider.Object, 2, configPath, null);
//
//            var product = sut.SourceConfig();
//
//            Console.WriteLine(product.ToString());
//        }
//
//        [Test]
//        [TestCase("32", "41.21", "KingKrab", "false")]
//        [TestCase("91", "19.01", "QueenBee", "true")]
//        [TestCase("-36", "4", "PrinceCharming", "false")]
//        public void ParseTypeTest(string intValue, string doubleValue, string stringValue, string boolValue)
//        {
//            SampleConfig config = new SampleConfig();
//
//            var source = new Dictionary<string, string>()
//            {
//                {"SampleStringProp", stringValue},
//                {"SampleDoubleProp", doubleValue},
//                {"SampleIntProp", intValue},
//                {"SampleBoolProp", boolValue}
//            };
//
//            foreach (var configProp in config.GetType().GetProperties())
//                config.SetProperty(configProp, source[configProp.Name]);
//
//            config.SampleIntProp.Should().Be(int.Parse(intValue));
//            config.SampleBoolProp.Should().Be(bool.Parse(boolValue));
//            config.SampleDoubleProp.Should().Be(double.Parse(doubleValue));
//            config.SampleStringProp.Should().Be(stringValue);
//        }
    }
}