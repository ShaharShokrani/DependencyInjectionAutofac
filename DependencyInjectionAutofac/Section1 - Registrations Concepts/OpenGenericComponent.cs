using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyInjectionAutofac
{
    public class OpenGenericComponent
    {
        public static void Run()
        {
            //If we need to use a generic collection:
            //IList<T> --> List<T>
            //IList<int> --> List<int>

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));
            IContainer container = builder.Build();

            var myList = container.Resolve<IList<int>>();
            Console.WriteLine(myList.GetType().Name);
            Console.WriteLine(myList.GetType());
        }
    }
}
