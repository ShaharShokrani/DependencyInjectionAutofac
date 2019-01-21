using System;
using System.Collections.Generic;
using Autofac;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    [TestClass]
    public class Section1
    {
        [TestMethod]
        public void ChoiceOfContructor()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //If we leave this two lines 
            builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            builder.RegisterType<EmailLog>().As<ILog>().PreserveExistingDefaults(); //Only changed this line of code in order to switch between ConsoleLog and EmailLog.

            //Without this line an exception will be thrown (missing component):
            builder.RegisterType<Engine>();
            ////In case we would like using specific constructor:
            builder.RegisterType<Car>().UsingConstructor(typeof(Engine));

            builder.RegisterType<Car>();

            IContainer container = builder.Build();
            Car car = container.Resolve<Car>();

            car.Go();
        }

        [TestMethod]
        public void DefaultRegistration()
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

        [TestMethod]
        public void LambdaExpressionComponent()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void OpenGenericComponent()
        {
            //If we need to use a generic collection:
            //IList<T> --> List<T>
            //IList<int> --> List<int>

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));
            IContainer container = builder.Build();

            var myList = container.Resolve<IList<int>>();
            Console.WriteLine(myList.GetType().Name);
            Console.WriteLine(myList.GetType());
        }

        [TestMethod]
        public void RegisteringInstances()
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

        [TestMethod]
        public void RegisteringTypes()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            builder.RegisterType<Engine>(); //Without this line an exception will be thrown (missing component)
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();

            car.Go();
        }

        [TestMethod]
        public void WithoutDI()
        {
            var log = new ConsoleLog();

            //Specify the log in each of the objects.
            var engine = new Engine(log);
            var car = new Car(engine, log);

            car.Go();
        }


    }
}
