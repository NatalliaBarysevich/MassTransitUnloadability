using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitUnloadability
{
    /// <summary>
    /// Add some classes to service collection from extern library 
    /// Library will be unloaded correctly
    /// </summary>
    public class HostRunnerWithSimpleServices : AbstractHostRunner
    {
        protected override void ConfigureServices(IServiceCollection serviceCollection, Assembly externalAssembly)
        {
            var externalTypes = externalAssembly.GetTypes();
            foreach (var externalType in externalTypes)
            {
                serviceCollection.AddSingleton(externalType);
            }
        }
    }
}
