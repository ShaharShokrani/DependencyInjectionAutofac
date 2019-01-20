using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class DefaultRegistration
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //If we leave this two lines 
            builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            builder.RegisterType<EmailLog>().As<ILog>().PreserveExistingDefaults(); //Only changed this line of code in order to switch between ConsoleLog and EmailLog.

            ////The ConsoleLog could not be resolved unless we register it as self and the resolve will be available to ConsoleLog and ILog.
            //builder.RegisterType<ConsoleLog>().As<ILog>().AsSelf();
            //ConsoleLog consoleLog = container.Resolve<ConsoleLog>();

            builder.RegisterType<Engine>(); 
            builder.RegisterType<Car>();

            IContainer container = builder.Build();
            Car car = container.Resolve<Car>();

            car.Go();
        }
    }
}
