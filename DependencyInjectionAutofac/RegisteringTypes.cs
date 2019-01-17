﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class RegisteringTypes
    {
        public static void Run()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //If we leave this two lines 
            //builder.RegisterType<ConsoleLog>().As<ILog>(); //If someone ask for ILog give them ConsoleLog
            builder.RegisterType<EmailLog>().As<ILog>(); //Only changed this line of code in order to switch between ConsoleLog and EmailLog.

            //Without this line an exception will be thrown (missing component):
            builder.RegisterType<Engine>(); 
            builder.RegisterType<Car>();

            IContainer container = builder.Build();
            Car car = container.Resolve<Car>();

            car.Go();
        }
    }
}
