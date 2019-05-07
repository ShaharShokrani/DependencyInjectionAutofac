using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.OwnedInstances;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    [TestClass]
    public partial class Section4
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

        class ControlledInstantiationReport
        {
            private Owned<ConsoleLog> _log;

            public ControlledInstantiationReport(Owned<ConsoleLog> log)
            {
                this._log = log;
                Console.WriteLine("Controlled Instantiation component created");
            }

            public void Report()
            {
                this._log.Value.Write("Log started...");
                this._log.Dispose();
            }
        }

        [TestMethod]
        public void ControlledInstantiation()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<ControlledInstantiationReport>();

            using (IContainer container = builder.Build())
            {
                ControlledInstantiationReport report = container.Resolve<ControlledInstantiationReport>();
                report.Report();

                //OUTPUT:
                //Controlled Instantiation component created
                //Log started
            }
        }

        class DynamicInstantiationReport
        {
            private Func<ConsoleLog> _log;

            public DynamicInstantiationReport(Func<ConsoleLog> log)
            {
                this._log = log;
                Console.WriteLine("Dynamic Instantiation component created");
            }

            public void Report()
            {
                this._log().Write("Log started...");
                this._log().Write("Log started...");
            }
        }

        [TestMethod]
        public void DynamicInstantiation()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<DynamicInstantiationReport>();

            using (IContainer container = builder.Build())
            {
                DynamicInstantiationReport report = container.Resolve<DynamicInstantiationReport>();
                report.Report();

                //OUTPUT:
                //Dynamic Instantiation component created
                //Log started...
                //Log started...
            }
        }

        class ParameterizedInstantiationReport
        {
            private Func<ConsoleLog> _log;
            private Func<string, SMSLog> _smsLog;

            public ParameterizedInstantiationReport(Func<ConsoleLog> log, Func<string, SMSLog> smsLog)
            {
                this._log = log;
                this._smsLog = smsLog;
                Console.WriteLine("Parameterized Instantiation component created");
            }

            public void Report()
            {
                this._log().Write("Log started...");
                this._log().Write("Log started...");

                this._smsLog("+123456789").Write("Sms log started...");
            }
        }

        [TestMethod]
        public void ParameterizedInstantiation()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>();
            builder.RegisterType<SMSLog>();
            builder.RegisterType<ParameterizedInstantiationReport>();

            using (IContainer container = builder.Build())
            {
                ParameterizedInstantiationReport report = container.Resolve<ParameterizedInstantiationReport>();
                report.Report();

                //OUTPUT:
                //Parameterized Instantiation component created
                //Log started...
                //Log started...
                //SMS has been sent to + 123456789: Sms log started...
            }
        }
    }
}
