using GreenPipes.Internals.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace MassTransitUnloadability
{
    /// <summary>
    /// Abstract class contains logic with loading and unloading extern assemblies
    /// There are two derived classes
    /// <see cref="HostRunnerWithMassTransit"/> and <see cref="HostRunnerWithSimpleServices"/>
    /// contain logic to demonstrate
    /// </summary>
    public abstract class AbstractHostRunner
    {
        private IHost _host;
        private AssemblyLoadContext _assemblyLoadContext = new TestAssemblyLoadContext(); //https://docs.microsoft.com/en-us/dotnet/standard/assembly/unloadability



        /// <summary>
        /// Load assembly to domain and create host.
        /// Configure it using <see cref="ConfigureServices"/> from offspring classes.
        /// </summary>
        /// <param name="dll">File with external assembly</param>
        public void Create(FileInfo dll)
        {
            var loadedAssembly = _assemblyLoadContext.LoadFromAssemblyPath(dll.FullName); //assembly load to domain

            _host = new HostBuilder()
            .ConfigureServices(s =>
            {
                ConfigureServices(s, loadedAssembly);
            })
            .Build();
        }

        protected abstract void ConfigureServices(IServiceCollection serviceCollection,
            Assembly externalAssembly);

        /// <summary>
        /// Start host
        /// </summary>
        public void Start()
        {
            if (_host != null)
                _host.StartAsync().Wait();
        }

        /// <summary>
        /// Stop and dispose host and unload assemblies
        /// </summary>
        public void Stop()
        {
            if (_host != null)
            {
                _host.StopAsync().Wait();
                _host.Dispose();
                _host = null;
                _assemblyLoadContext.Unload();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
