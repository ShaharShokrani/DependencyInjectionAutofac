using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{    
    public partial class Section4
    {
        private class Reporting
        {
            private IEnumerable<IOptionFactory<IOption>> _allOptionsFactories;

            public Reporting(IEnumerable<IOptionFactory<IOption>> allOptionsFactories)
            {
                if (allOptionsFactories == null)
                {
                    throw new ArgumentNullException("Parameter:" + nameof(allOptionsFactories));
                }
                this._allOptionsFactories = allOptionsFactories;
            }

            public void Report()
            {
                foreach (var optionsFactories in _allOptionsFactories)
                {
                    Console.WriteLine(optionsFactories.GetType());
                }
            }
        }

        internal interface IOption
        {
            void Do();
        }
        interface IOptionFactory<out T> where T : IOption
        {
            void DoOption();
        }

        private class IOption1Factory1<SomeOption> : IOptionFactory<IOption>
        {
            public void DoOption()
            {
                Console.Write("IOption1Factory1");
            }
        }
        private class IOption1Factory2<SomeOption> : IOptionFactory<IOption>
        {
            public void DoOption()
            {
                Console.Write("IOption1Factory2");
            }
        }

        private class SomeOption : IOption
        {
            public void Do()
            {
                Console.WriteLine("Do Option1");
            }
        }

        [TestMethod]
        public void GenericEnumerationsTest()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<IOption1Factory1<SomeOption>>().As<IOptionFactory<IOption>>();
            builder.RegisterType<IOption1Factory2<SomeOption>>().As<IOptionFactory<IOption>>();
            builder.RegisterType<Reporting>();

            using (var container = builder.Build())
            {
                container.Resolve<Reporting>().Report();
            }

            //OUTPUT:

            //Enumerations + IOption1Factory1`1[Enumerations + SomeOption]
            //Enumerations + IOption1Factory2`1[Enumerations + SomeOption]
        }
    }
}
