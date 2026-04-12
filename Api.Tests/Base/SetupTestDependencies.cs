using Api.Tests.Modules;
using Configuration.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Tests.Tools.Logger;

namespace Api.Tests.Base
{
    /// <summary>
    /// Registers framework and test dependencies for Reqnroll using
    /// Microsoft.Extensions.DependencyInjection.
    /// </summary>
    public static class SetupTestDependencies
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateServices()
        {
            IConfiguration config = new Configurator().GetConfig();
            var configHelper = new ConfigHelper(config);

            var loggerType = configHelper.GetRequiredString(
                ConfigKeys.GlobalParametersSection,
                ConfigKeys.LoggerSection,
                ConfigKeys.LoggerType);

            var logger = LoggerFactory.Create(loggerType);

            var services = new ServiceCollection();

            // Core framework services
            services.AddSingleton(config);
            services.AddSingleton(configHelper);
            services.AddSingleton<ILog>(logger);

            // API modules
            services.AddPetModule();

            return services;
        }
    }
}