using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bit.Core.Contracts;
using Bit.Core.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Bit.Client.Web.Blazor.Implementation
{
    public class BitServiceProviderFactory(Action<IDependencyManager, IServiceCollection, ContainerBuilder> configureAction) : IServiceProviderFactory<ContainerBuilder>
    {
        private IContainer _container = default!;

        public virtual ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Properties["services"] = services;

            AutofacDependencyManager dependencyManager = new AutofacDependencyManager();
            dependencyManager.UseContainerBuilder(containerBuilder);
            ((IServiceCollectionAccessor)dependencyManager).ServiceCollection = services;

            containerBuilder.Properties["dependencyManager"] = dependencyManager;

            containerBuilder.Register(c => _container).SingleInstance();

            configureAction?.Invoke(dependencyManager, services, containerBuilder);

            containerBuilder.Populate(services);

            return containerBuilder;
        }

        public virtual IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            _container = containerBuilder?.Build()!;

            return new AutofacServiceProvider(_container);
        }
    }
}
