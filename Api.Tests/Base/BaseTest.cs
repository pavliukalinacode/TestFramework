using Api.Services.Components;
using Configuration.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tests.Tools.Logger;

namespace API.Tests.Base
{
    public abstract class BaseTest
    {
        private static readonly Lazy<ServiceProvider> _provider = new(BuildProvider);

        internal static IServiceProvider Provider => _provider.Value;

        internal static void DisposeRootProvider()
        {
            if (_provider.IsValueCreated)
            {
                _provider.Value.Dispose();
            }
        }

        private static ServiceProvider BuildProvider()
        {
            var config = new Configurator().GetConfig();
            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(config);

            var helper = new ConfigHelper(config);
            services.AddSingleton(helper);

            RegisterLogging(services, helper);
            RegisterServices(services, helper);

            services.AddScoped<TestContext>();

            return services.BuildServiceProvider();
        }

        private static void RegisterLogging(IServiceCollection services, ConfigHelper helper)
        {
            var loggerType = helper.GetRequiredString(ConfigKeys.GlobalParametersSection, ConfigKeys.LoggerSection, ConfigKeys.LoggerType);
            services.AddSingleton<ILog>(LoggerFactory.Create(loggerType));
        }

        private static void RegisterServices(IServiceCollection services, ConfigHelper helper)
        {
            services.AddHttpClient<PetService>(client =>
            {
                var baseUrl = helper.GetRequiredString(ConfigKeys.ScenariosSection, ConfigKeys.PetTests, ConfigKeys.BaseUrl);
                client.BaseAddress = new Uri(baseUrl);
            });
        }
    }
}