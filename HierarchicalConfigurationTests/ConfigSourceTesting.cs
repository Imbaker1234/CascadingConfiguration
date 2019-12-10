﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
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

        [Test]
        [TestCase("32", "41.21", "KingKrab", "false")]
        [TestCase("91", "19.01", "QueenBee", "true")]
        [TestCase("-36", "4", "PrinceCharming", "false")]
        public void ParseTypeTest(string intValue, string doubleValue, string stringValue, string boolValue)
        {
            SampleConfig config = new SampleConfig();

            var source = new Dictionary<string, string>()
            {
                {"SampleStringProp", stringValue},
                {"SampleDoubleProp", doubleValue},
                {"SampleIntProp", intValue},
                {"SampleBoolProp", boolValue}
            };

            foreach (var configProp in typeof(SampleConfig).GetProperties())
            {
                if (configProp.Name == "FullyConfigured") continue; 
                ConvertAndSetValue(config, configProp, source[configProp.Name]);
            }

            config.SampleIntProp.Should().Be(int.Parse(intValue));
            config.SampleBoolProp.Should().Be(bool.Parse(boolValue));
            config.SampleDoubleProp.Should().Be(double.Parse(doubleValue));
            config.SampleStringProp.Should().Be(stringValue);
        }

        public void ConvertAndSetValue(object classToPopulate, PropertyInfo property, string value)
        {
            //Get the type we need to cast to.
            Enum.TryParse(property.PropertyType.Name, true, out TypeCode enumValue);
            //Cast to that type
            var convertedValue = Convert.ChangeType(value, enumValue);
            //Assign
            property.SetValue(classToPopulate, convertedValue);
        }
    }
}