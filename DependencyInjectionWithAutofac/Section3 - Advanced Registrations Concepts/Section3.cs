using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    [TestClass]
    public partial class Section3
    {
        [TestMethod]
        public void PassingParametersToRegister_NamedParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter("phoneNumber", "+123456789");

            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void PassingParametersToRegister_TypedParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(new TypedParameter(typeof(string), "+123456789"));

            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void PassingParametersToRegister_ResolvedParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<SMSLog>()
                .As<ILog>()
                .WithParameter(
                    new ResolvedParameter(
                        //predicate
                        (pi, ctx) => pi.ParameterType == typeof(string) && pi.Name == "phoneNumber",
                        //value accessor
                        (pi, ctx) => "+123456789" //We even could use the ctx to resolve a type.
                    )
                );


            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void PassingParametersToRegister_LambdaParameter()
        {
            ContainerBuilder builder = new ContainerBuilder();

            Random random = new Random();
            builder.Register((c, p) => new SMSLog(p.Named<string>("phoneNumber")))
                .As<ILog>();

            Console.WriteLine("About to build a container...");
            IContainer container = builder.Build();

            ILog log = container.Resolve<ILog>(new NamedParameter("phoneNumber", random.Next().ToString()));
            log.Write("Testing");
        }

        [TestMethod]
        public void PropertyInjection_NoOutput()
        {
            ContainerBuilder builder = new ContainerBuilder();
            
            builder.RegisterType<Parent>();
            builder.RegisterType<Child>();

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent); //This will result in no output.
        }

        [TestMethod]
        public void PropertyInjection_PropertiesAutowired()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Parent>();
            builder.RegisterType<Child>().PropertiesAutowired(); //The autofac will go to every single property and try to resolve it.

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent); //This will result in no output.
        }

        [TestMethod]
        public void PropertyInjection_WithProperty()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Parent>();
            builder.RegisterType<Child>()
                .WithProperty("Parent", new Parent());

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent);
        }

        [TestMethod]
        public void MethodInjection_WithProperty()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Parent>();
            builder.Register(c =>
            {
                var child = new Child();
                child.SetParent(c.Resolve<Parent>());
                return child;
            });
                
            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent); 
        }

        [TestMethod]
        public void MethodInjection_OnActivated()
        {
            //Provide an event handler that about to fired when registering
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<Parent>();
            builder.RegisterType<Child>()
                .OnActivated((IActivatedEventArgs<Child> e) =>
                {
                    var p = e.Context.Resolve<Parent>();
                    e.Instance.SetParent(p);
                });

            var container = builder.Build();
            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent); 
        }

        [TestMethod]
        public void ScanningForTypes_AsSelf()
        {
            //Register in bulk
            var assembly = Assembly.GetExecutingAssembly();

            ContainerBuilder builder = new ContainerBuilder();

            //builder.RegisterAssemblyTypes(assembly)
            //    .Where(t => t.Name.EndsWith("Log"))
            //    .Except<SMSLog>()
            //    .Except<ConsoleLog>(c => c.As<ILog>().SingleInstance())
            //    .As<ILog>()
            //    .WithParameter(new TypedParameter(typeof(string), "+123456789"));

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.Name.EndsWith("Log"))
                .Except<SMSLog>()
                .Except<ConsoleLog>(c => c.As<ILog>().SingleInstance())
                .AsSelf();

            builder.RegisterType<Engine>();
            builder.RegisterType<Car>();

            IContainer container = builder.Build();

            Car car = container.Resolve<Car>();
            car.Go();
        }

        [TestMethod]
        public void ScanningForTypes_GetInterfaces()
        {
            //Register in bulk
            var assembly = Assembly.GetExecutingAssembly();

            ContainerBuilder builder = new ContainerBuilder();


            //builder.RegisterType<ConsoleLog>().As<ILog>();

            builder.RegisterAssemblyTypes(assembly)
            .Where(t => t.Name.EndsWith("Log")).As<ILog>().AsSelf();
            //.Except<SMSLog>()
            //.As(t => t.GetInterfaces()[0]); //the first interface which they are implemented.

            //builder.RegisterType<Engine>();
            //builder.RegisterType<Car>();

            IContainer container = builder.Build();

            //Parent parent = container.Resolve<Parent>();
            SMSLog car = container.Resolve<SMSLog>();
//            Engine car = container.Resolve<Engine>();
            //car.Go();
        }

        [TestMethod]
        public void ScanningForModule()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(Section3).Assembly);
            //builder.RegisterAssemblyModules<ParentChildModule>(typeof(Section3).Assembly);

            IContainer container = builder.Build();

            var parent = container.Resolve<Child>().Parent;
            Console.WriteLine(parent); //This will result in output.
        }

        //[TestMethod]
        //public void RegisterAssemblyTypesTest()
        //{
        //    var assembly = Assembly.GetExecutingAssembly();

        //    ContainerBuilder builder = new ContainerBuilder();

        //    builder.RegisterAssemblyTypes(assembly);

        //    IContainer container = builder.Build();

        //    MyClass car = container.Resolve<MyClass>();
        //}
    }
}
