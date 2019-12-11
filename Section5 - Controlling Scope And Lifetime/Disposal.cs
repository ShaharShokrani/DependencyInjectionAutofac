using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Indexed;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{    
    [TestClass]
    public partial class Section5
    {
        [TestMethod]
        public void Dispose1Test()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>();
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<ConsoleLog>();
            }
            //Should output "Consoe log has been disposed."
        }
        [TestMethod]
        public void Dispose2Test()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsoleLog>();
            builder.RegisterInstance(new ConsoleLog());
            using (var container = builder.Build())
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    scope.Resolve<ConsoleLog>();
                }
            }
            //Should output "Consoe log has been disposed."
        }
        [TestMethod]
        public void Dispose_InstanceTest()
        {
            var builder = new ContainerBuilder();
            //builder.RegisterType<ConsoleLog>();
            builder.RegisterInstance(new ConsoleLog());
            var container = builder.Build();
            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<ConsoleLog>();
            }            
            //Should output nothing.
        }
        [TestMethod]
        public void Dispose_ExternallyOwnedTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ConsoleLog>().ExternallyOwned();            
            var container = builder.Build();
            var scope = container.BeginLifetimeScope();
            var consoleLog = scope.Resolve<ConsoleLog>();
            try
            {
                //consoleLog;
            }
            finally
            {
                if (consoleLog != null)
                    consoleLog.Dispose();
            };

            //Should output ....
        }
        [TestMethod]
        public void Dispose_ExternallyOwned2Test()
        {
            var builder = new ContainerBuilder();
            ConsoleLog consoleLog = new ConsoleLog();
            builder.RegisterInstance(consoleLog).ExternallyOwned();
            var container = builder.Build();                       
            try
            {
                var scope = container.BeginLifetimeScope();
            }
            finally
            {
                if (consoleLog != null)
                    consoleLog.Dispose();
            };

            //Should output ....
        }
    }    
}