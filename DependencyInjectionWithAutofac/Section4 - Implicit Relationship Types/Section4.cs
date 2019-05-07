using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    [TestClass]
    public class Section4
    {
        class DelayedInstantiationReport
        {
            private Lazy<ConsoleLog> _log;

            public DelayedInstantiationReport(Lazy<ConsoleLog> log)
            {
                this._log = log;
                Console.WriteLine("Delayed Instantiation component created");
            }

            public void Report()
            {
                this._log.Value.Write("Log started");
            }
        }

        [TestMethod]
        public void DelayedInstantiation()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<DelayedInstantiationReport>();

            using (IContainer container = builder.Build())
            {
                DelayedInstantiationReport report = container.Resolve<DelayedInstantiationReport>();
                report.Report();
                
                //OUTPUT:
                //Delayed Instantiation component created
                //Log started
            }
        }
    }
}
