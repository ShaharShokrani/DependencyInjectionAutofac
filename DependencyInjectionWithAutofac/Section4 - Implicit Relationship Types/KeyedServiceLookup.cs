using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Indexed;
using Autofac.Features.Metadata;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{    
    public partial class Section4
    {
        class KeyedServiceLookup
        {
            private IIndex<string, ILog> _logs;

            public KeyedServiceLookup(IIndex<string, ILog> log)
            {
                this._logs = log;
                Console.WriteLine("IIndex Instantiation component created");
            }

            public void Report()
            {
                this._logs["sms"].Write($"Report Weakly Typed Log started...");                            
            }
        }

        [TestMethod]
        public void KeyedServiceLookupTest()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>().Keyed<ILog>("cmd");
            builder.Register((c) => new SMSLog("+123456789")).Keyed<ILog>("sms");

            builder.RegisterType<KeyedServiceLookup>();

            using (IContainer container = builder.Build())
            {
                KeyedServiceLookup report = container.Resolve<KeyedServiceLookup>();
                report.Report();

                //OUTPUT:
                //IIndex Instantiation component created
                //SMS has been sent to + 123456789: Report Weakly Typed Log started...
            }
        }
    }
}
