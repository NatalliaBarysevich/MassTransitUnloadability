using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GreenPipes.Internals.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransitUnloadability
{
    /// <summary>
    /// Use <see cref="RegistrationExtensions.AddConsumers(IRegistrationConfigurator, Type[])"/>
    /// So external library can't be unload
    /// </summary>
    public class HostRunnerWithMassTransit : AbstractHostRunner
    {
        /// <summary>
        /// Use <see cref="RegistrationExtensions.AddConsumers(IRegistrationConfigurator, Type[])"/> extension and other class from MassTransit 
        /// use lot of static caches contain consumer and message types
        /// For example <see cref="MassTransit.Util.TypeMetadataCache"/> and <see cref="MassTransit.Util.AssemblyTypeCache"/>
        /// Also there is lot of classes contain private dictionaries to consumers and message types.
        /// For example
        /// 1) <see cref="MassTransit.ExtensionsDependencyInjectionIntegration.Configuration.Registration.ServiceCollectionConfigurator"/>
        /// derived from <see cref="MassTransit.Registration.RegistrationConfigurator"/>.
        /// The last has lot of caches contain classes who referred to <see cref="Type"/>, or it is generic classes. So they hold references to external libraries. 
        /// </summary>
        protected override void ConfigureServices(IServiceCollection serviceCollection,
            Assembly externalAssembly)
        {
            var consumerTypes = externalAssembly.GetTypes().Where(t => t.HasInterface(typeof(IConsumer<>))).ToArray();
            serviceCollection.AddMassTransit(x => x.AddConsumers(consumerTypes)); 
        }
    }
}
