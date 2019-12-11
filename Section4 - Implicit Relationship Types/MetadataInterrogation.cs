using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Metadata;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{    
    public partial class Section4
    {
        class MetadataInterrogationReport_WeaklyType
        {
            private Meta<ConsoleLog> _log;

            public MetadataInterrogationReport_WeaklyType(Meta<ConsoleLog> log)
            {
                this._log = log;
                Console.WriteLine("Metadata Instantiation component created");
            }

            public void Report()
            {
                this._log.Value.Write($"Report Weakly Typed Log started...");
              
                if (_log.Metadata["mode"] as string == "verbode")
                {
                    this._log.Value.Write($"Log Weakly Typed verbose started... {DateTime.UtcNow}");
                }
            }
        }

        [TestMethod]
        public void MetadataInterrogation_WeaklyType()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>().WithMetadata("mode", "verbode");
            builder.RegisterType<MetadataInterrogationReport_WeaklyType>();

            using (IContainer container = builder.Build())
            {
                MetadataInterrogationReport_WeaklyType report = container.Resolve<MetadataInterrogationReport_WeaklyType>();
                report.Report();

                //OUTPUT:
                //Metadata Instantiation component created
                //Report Weakly Typed Log started...
                //Log Weakly Typed verbose started... 07 / 05 / 2019 11:26:06
            }
        }

        class MetadataInterrogationReport_StronglyType
        {
            private Meta<ConsoleLog, Settings> _log;

            public MetadataInterrogationReport_StronglyType(Meta<ConsoleLog, Settings> log)
            {
                this._log = log;
                Console.WriteLine("Metadata Instantiation component created");
            }

            public void Report()
            {
                this._log.Value.Write($"Report Strongly Typed Log started...");

                if (_log.Metadata.LogMode == "verbode")
                {
                    this._log.Value.Write($"Log Strongly Typed verbose started... {DateTime.UtcNow}");
                }
            }
        }

        class Settings
        {
            public string LogMode { get; set; }
        }

        [TestMethod]
        public void MetadataInterrogation_StronglyType()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>().WithMetadata<Settings>((c) => c.For(x => x.LogMode, "verbose"));
            builder.RegisterType<MetadataInterrogationReport_StronglyType>();

            using (IContainer container = builder.Build())
            {
                MetadataInterrogationReport_StronglyType report = container.Resolve<MetadataInterrogationReport_StronglyType>();
                report.Report();

                //OUTPUT:
                //Metadata Instantiation component created
                //Report Strongly Typed Log started...
            }
        }
    }
}
