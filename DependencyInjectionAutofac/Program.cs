﻿using Autofac;
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

    public class Engine
    {
        private ILog _log;
        private int _id;

        public Engine(ILog log)
        {
            this._log = log;
            this._id = new Random().Next();
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

            builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            builder.RegisterType<Engine>(); //Without this line an exception will be thrown (missing component).
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            ////The ConsoleLog could not be resolved unless we register it as self and the resolve will be available to ConsoleLog and ILog.
            //ConsoleLog consoleLog = container.Resolve<ConsoleLog>();

            Car car = container.Resolve<Car>();
            car.Go();

            //var log = new ConsoleLog();

            ////Specify the log in each of the objects.
            //var engine = new Engine(log);
            //var car = new Car(engine, log);
        }
    }
}
