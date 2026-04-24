using Api.Services.Modules;
using Configuration.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Tests.Tools.Logger;
using UI.Apps.SauceDemo.Module;
using UI.Framework.Auth;
using UI.Framework.Base;

namespace UI.Tests.Base;

public static class SetupTestDependencies
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();

        var configuration = new Configurator().GetConfig();
        var configHelper = new ConfigHelper(configuration);

        var loggerType = configHelper.GetRequired<string>(ConfigKeys.GlobalParametersSection, ConfigKeys.LoggerSection, ConfigKeys.LoggerType);
        var logger = LoggerFactory.Create(loggerType);

        // Core services
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton(configHelper);
        services.AddSingleton<ILog>(logger);

        services.AddSingleton(sp =>
        {
            var configHelper = sp.GetRequiredService<ConfigHelper>();

            return configHelper.GetRequiredSection<PlaywrightOptions>(
                ConfigKeys.GlobalParametersSection,
                ConfigKeys.PlaywrightSection);
        });

        services.AddSingleton<PlaywrightBrowserHost>();
        services.AddScoped<BrowserSession>();

        // API
        services.AddPetModule();

        // UI
        services.AddSingleton<IAuthStateProvider, AuthStateProvider>();
        services.AddSauceDemoModule();

        return services;
    }
}