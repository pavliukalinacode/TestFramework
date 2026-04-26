using Api.Services.Modules;
using Configuration.Config;
using Logging.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reqnroll.Microsoft.Extensions.DependencyInjection;
using Tests.Tools.Webhook;

namespace Api.Tests.Base;

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

        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton(configHelper);
        services.AddSingleton<ILog>(logger);

        services.AddScoped<TestWebhookServer>();
        services.AddPetModule();

        return services;
    }
}