using Apex.Core.Caching;
using Apex.Services.Logs;
using LightInject.Microsoft.DependencyInjection;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Apex.Websites
{
    public static class StartupServices
    {
        public static IServiceProvider AddInternalServices(this IServiceCollection services)
        {
            var container = new ServiceContainer(new ContainerOptions
            {
                EnablePropertyInjection = false
            });

            container
                .SetDefaultLifetime<PerScopeLifetime>()
                // Infrastructure.
                .Register<IMemoryCacheService, MemoryCacheService>()
                // Logs.
                .Register<IActivityLogTypeService, ActivityLogTypeService>()
                .Register<IActivityLogService, ActivityLogService>()
                .Register<ILogService, LogService>();

            return container.CreateServiceProvider(services);
        }
    }
}