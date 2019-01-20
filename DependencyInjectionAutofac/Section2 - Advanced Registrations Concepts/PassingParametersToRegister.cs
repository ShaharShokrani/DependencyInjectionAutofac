using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class PassingParametersToRegister
    {
        public static void Run_NamedParameter()
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

        public static void Run_TypedParameter()
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

        public static void Run_ResolvedParameter()
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

        public static void Run_LambdaParameter()
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
