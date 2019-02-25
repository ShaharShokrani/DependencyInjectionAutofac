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

    public class SMSLog : ILog
    {
        private string _phoneNumber;

        public SMSLog(string phoneNumber)
        {
            this._phoneNumber = phoneNumber;
        }

        public void Write(string message)
        {
            Console.WriteLine($"SMS has been sent to {this._phoneNumber}: {message}");
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

    public class Parent
    {
        public override string ToString()
        {
            return "I'm your parent!";
        }
    }

    public class Child
    {
        public string Name { get; set; }
        public Parent Parent { get; set; }

        public void SetParent(Parent parent)
        {
            this.Parent = parent;
        }
    }

    public interface IMyChild
    {
        void Foo();
    }

    public class MyChild : IMyChild
    {
        public void Foo()
        {          
        }
    }

    public class MyParent
    {
        private IMyChild _child;

        public MyParent(IMyChild child)
        {
            this._child = child;
        }
    }
}
