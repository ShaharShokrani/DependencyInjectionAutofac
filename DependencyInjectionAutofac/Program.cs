using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public interface ILog
    {
        void Write(string message);
    }

    public class ConsoleLog : ILog
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class EmailLog : ILog
    {
        private string _adminEmail = "admin@foo.com";

        public void Write(string message)
        {            
            Console.WriteLine($"Email has been sent to {_adminEmail}: {message}");
        }
    }

    public class Engine
    {
        private ILog _log;
        private int _id;

        public Engine(ILog log)
        {
            this._log = log;
            this._id = new Random().Next();
        }

        public Engine(ILog log, int Id)
        {
            this._log = log;
            this._id = Id;
        }

        public void Ahead(int power)
        {
            _log.Write($"Engine [{_id}] ahead {power} ");
        }
    }

    public class Car
    {
        private Engine _engine;
        private ILog _log;

        public Car(Engine engine)
        {
            this._engine = engine;
            this._log = new EmailLog();
        }

        public Car(Engine engine, ILog log)
        {
            this._engine = engine;
            this._log = log;
        }

        public void Go()
        {
            _engine.Ahead(100);
            _log.Write($"Car going forward...");
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();

            //If we leave this two lines 
            //builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            //builder.RegisterType<EmailLog>().As<ILog>(); //Only changed this line of code in order to switch between ConsoleLog and EmailLog.

            ////In case of unit testing, we would like to test specific instance:
            //var log = new ConsoleLog();
            //builder.RegisterInstance(log).As<ILog>();

            ////For testing, we would like to check a specific Id number:            
            //var engine = new Engine(new ConsoleLog(), 123);
            //builder.RegisterInstance(engine);

            //builder.Register((IComponentContext c) => new Engine(c.Resolve<ILog>(), 123));



            //builder.RegisterType<Engine>(); //Without this line an exception will be thrown (missing component).
            //builder.RegisterType<Car>();

            ////In case we would like using specific constructor:
            //builder.RegisterType<Car>().UsingConstructor(typeof(Engine));

            ////The ConsoleLog could not be resolved unless we register it as self and the resolve will be available to ConsoleLog and ILog.
            //ConsoleLog consoleLog = container.Resolve<ConsoleLog>();

            //If we need to use a generic collection:
            //IList<T> --> List<T>
            //IList<int> --> List<int>
            builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));


            IContainer container = builder.Build();

            //Car car = container.Resolve<Car>();
            //car.Go();

            var myList = container.Resolve<IList<int>>();
            Console.WriteLine(myList.GetType().Name);
            Console.WriteLine(myList.GetType());

            //var log = new ConsoleLog();

            ////Specify the log in each of the objects.
            //var engine = new Engine(log);
            //var car = new Car(engine, log);


        }
    }
}
