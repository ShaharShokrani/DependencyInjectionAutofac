using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class RegisteringInstances
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //In case of unit testing, we would like to test specific instance:
            var log = new ConsoleLog();
            builder.RegisterInstance(log).As<ILog>();

            //For testing, we would like to check a specific Id number:            
            var engine = new Engine(new ConsoleLog(), 123);
            builder.RegisterInstance(engine);

            builder.RegisterType<Car>();

            IContainer container = builder.Build();
            Car car = container.Resolve<Car>();

            car.Go();
        }
    }
}
