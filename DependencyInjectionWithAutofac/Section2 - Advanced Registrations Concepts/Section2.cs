using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    [TestClass]
    public class Section2
    {
        [TestMethod]
        public void PassingParametersToRegister_NamedParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter("phoneNumber", "+123456789");

            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void PassingParametersToRegister_TypedParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(new TypedParameter(typeof(string), "+123456789"));

            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void PassingParametersToRegister_ResolvedParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(
                    new ResolvedParameter(
                        //predicate
                        (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "phoneNumber",
                        //value accessor
                        (pi, ctx) => "+123456789" //We even could use the ctx to resolve a type.
                    )
                );


            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void PassingParametersToRegister_LambdaParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            Random random = new Random();
            builder.Register((c, p) => new SMSLog(p.Named<string>("phoneNumber")))
                .As<ILog>();

            Console.WriteLine("About to build a container...");
            IContainer container = builder.Build();

            ILog log = container.Resolve<ILog>(new NamedParameter("phoneNumber", random.Next().ToString()));
            log.Write("Testing");
        }
    }
}
