using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Features.Indexed;
using DependencyInjectionAutofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DependencyInjectionWithAutofac
{    
    public interface IResource { }
    class SingletonResource : IResource { }
    public class InstancePerDependencyResource : IResource,IDisposable
    {
        public InstancePerDependencyResource()
        {
            Console.WriteLine("Instance Per Dependency Created");
        }

        public void Dispose()
        {
            Console.WriteLine("Instance Per Dependency Disposed");
        }
    }

    //auto will try to populate the ienumarable with an instance of each component.
    //The singleton will be leaving 
    public class ResourceManager
    {
        public IEnumerable<IResource> Resources { get; set; }
        public ResourceManager(IEnumerable<IResource> resources)
        {
            this.Resources = resources;
        }
    }

    public partial class Section5
    {

        //It is when long live holds on to a short live component.
        //When we have an IEnumarable of something autofac will inject all of the descendants into the type
        [TestMethod]
        public void CaptiveDependencies1Test()
        {
            //Even thought that it is only Instance Per Dependency its lifetime depend uppon the lifetime of the singleton
            var builder = new ContainerBuilder();
            builder.RegisterType<ResourceManager>().SingleInstance();
            builder.RegisterType<SingletonResource>().As<IResource>().SingleInstance();
            builder.RegisterType<InstancePerDependencyResource>().As<IResource>();

            using (var container = builder.Build())
            {
                using (var scope = container.BeginLifetimeScope())
                {
                    scope.Resolve<ResourceManager>(); // singleton, it also happens to hold an intance dependency.
                }
            }

            //Should output:
            //Instance Per Dependency Created
            //Instance Per Dependency Disposed
        }
    }    
}