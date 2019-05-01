﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    [TestClass]
    public class Section3
    {
        private class C
        {
            public override string ToString()
            {
                return "I'm C";
            }
        }

        private class B
        {
            public B(C c)
            {
                this.C = c;
            }
            private C C { get; set; }

            public override string ToString()
            {
                return $"I'm B with C of {this.C}";
            }
        }

        private class A
        {
            public A(B b)
            {
                this.B = b;
            }

            private B B { get; set; }

            public override string ToString()
            {
                return $"I'm A with B of {this.B}";
            }
        }


        [TestMethod]
        public void ResolvingWithPolymorphism()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<A>();
            cb.Register<B>((c, p) => new B(p.TypedAs<C>()));
            using (var c = cb.Build())
            {
                B myB = c.Resolve<B>(TypedParameter.From(new C()));

                A myA = c.Resolve<A>(TypedParameter.From(myB));

                Console.Write(myA);
            }
        }
    }
}
