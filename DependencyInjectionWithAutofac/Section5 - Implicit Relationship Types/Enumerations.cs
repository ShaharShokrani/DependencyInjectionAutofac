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
    public class Enumerations
    {
        private class Reporting
        {
            private IList<ILog> _allLogs;

            public Reporting(IList<ILog> allLogs)
            {
                if (allLogs == null)
                {
                    throw new ArgumentNullException("Parameter:" + nameof(allLogs));
                }
                this._allLogs = allLogs;
            }

            public void Report()
            {
                foreach (var log in _allLogs)
                {
                    log.Write($"Hello, this is {log.GetType().Name}");
                }
            }
        }

        [TestMethod]
        public void PassingParametersToRegister_NamedParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new SMSLog("+123456789")).As<ILog>();
            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.RegisterType<EmailLog>().As<ILog>();
            builder.RegisterType<Reporting>();

            using (var container = builder.Build())
            {
                container.Resolve<Reporting>().Report();
            }
        }
    }
}
