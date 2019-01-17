﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class ChoiceOfContructor
    {
        public static void Run()
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
    }
}
