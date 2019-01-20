using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class LambdaExpressionComponent
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }
    }
}
