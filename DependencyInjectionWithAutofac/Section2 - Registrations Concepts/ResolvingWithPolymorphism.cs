using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{
    public partial class Section3
    {
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

                //OUTPUT: I'm A with B of I'm B with C of I'm C
            }
        }


        [TestMethod]
        public void ResolvingWithPolymorphism_OneLiner()
        {
            var cb = new ContainerBuilder();
            cb.Register<A>((c, p) =>
            {
                B myB = c.Resolve<B>(p);
                return new A(myB);
            });
            cb.Register<B>((c, p) => {
                C myC = p.Named<C>("myC");
                return new B(myC);
            });
            using (var c = cb.Build())
            {
                A myA = c.Resolve<A>(new NamedParameter("myC", new C()));

                Console.Write(myA);

                //OUTPUT: I'm A with B of I'm B with C of I'm C
            }
        }

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
    }
}
