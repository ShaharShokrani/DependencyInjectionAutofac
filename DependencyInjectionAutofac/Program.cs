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
            if (false)
            {
                WithoutDI.Run();
            }
            if (false)
            {
                RegisteringTypes.Run();
            }
            if (false)
            {
                ChoiceOfContructor.Run();
            }
            if (true)
            {
                RegisteringInstances.Run();
            }

            //builder.Register((IComponentContext c) => new Engine(c.Resolve<ILog>(), 123));




            //If we need to use a generic collection:
            //IList<T> --> List<T>
            //IList<int> --> List<int>
            //builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));


            //IContainer container = builder.Build();

            ////Car car = container.Resolve<Car>();
            ////car.Go();

            //var myList = container.Resolve<IList<int>>();
            //Console.WriteLine(myList.GetType().Name);
            //Console.WriteLine(myList.GetType());

        }
    }
}
