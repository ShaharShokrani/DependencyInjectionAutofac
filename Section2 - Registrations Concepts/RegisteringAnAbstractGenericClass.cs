using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    /// <summary>
    /// This code is meant to give answer to this question: https://stackoverflow.com/q/61285697/6844481
    /// </summary>
    public partial class Section2
    {
        [TestMethod]
        public void RegisteringAnAbstractGenericClassTest()
        {
            ContainerBuilder builder = new ContainerBuilder();            
            builder.RegisterType<TestHandler>().Named("myTestHandler", typeof(AbstractHandler<CommonArgs>));

            IContainer container = builder.Build();

            var testHandler = container.ResolveNamed("myTestHandler", typeof(AbstractHandler<CommonArgs>));
            Console.WriteLine(testHandler.ToString()); //OUTPUT: TestHandler has been activated!

            Assert.AreEqual(testHandler.ToString(), "TestHandler has been activated!");
        }


        private class CommonArgs
        {
        }
        private class TestArgs : CommonArgs
        {
        }
        private abstract class AbstractHandler<T> where T : CommonArgs, new()
        {
        }

        private class TestHandler : AbstractHandler<CommonArgs>
        {
            public override string ToString()
            {
                return "TestHandler has been activated!";
            }
        }
    }
}
