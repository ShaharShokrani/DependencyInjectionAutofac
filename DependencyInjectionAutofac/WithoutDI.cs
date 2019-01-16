using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class WithoutDI
    {
        public static void Run()
        {
            var log = new ConsoleLog();

            //Specify the log in each of the objects.
            var engine = new Engine(log);
            var car = new Car(engine, log);

            car.Go();
        }
    }
}
