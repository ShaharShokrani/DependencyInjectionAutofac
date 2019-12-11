using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{    
    public partial class Section4
    {
        private class EnumerationsReport
        {
            private IList<ILog> _allLogs;

            public EnumerationsReport(IList<ILog> allLogs)
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
        public void EnumerationsTest()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(c => new SMSLog("+123456789")).As<ILog>();
            builder.RegisterType<ConsoleLog>().As<ILog>();
            builder.RegisterType<EmailLog>().As<ILog>();
            builder.RegisterType<EnumerationsReport>();

            using (var container = builder.Build())
            {
                container.Resolve<EnumerationsReport>().Report();
            }
        }
    }
}
