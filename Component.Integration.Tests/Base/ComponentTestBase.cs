using Api.Services.Components;
using Configuration.Config;
using Logging.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Component.Integration.Tests.Base
{
    public abstract class ComponentTestBase
    {
        protected EventServiceFactory Factory = null!;

        private ServiceProvider serviceProvider = null!;

        [SetUp]
        public void SetUp()
        {
            Factory = new EventServiceFactory();

            var services = new ServiceCollection();

            RegisterCoreServices(services);
            RegisterApiServices(services);

            serviceProvider = services.BuildServiceProvider();
        }

        protected virtual void RegisterCoreServices(IServiceCollection services)
        {
            var configuration = new Configurator().GetConfig();
            var configHelper = new ConfigHelper(configuration);

            var loggerType = configHelper.GetRequired<string>(
                ConfigKeys.GlobalParametersSection,
                ConfigKeys.LoggerSection,
                ConfigKeys.LoggerType);

            var logger = LoggerFactory.Create(loggerType);

            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton(configHelper);
            services.AddSingleton<ILog>(logger);
        }

        protected virtual void RegisterApiServices(IServiceCollection services)
        {
            services.AddHttpClient<EventsService>((sp, client) =>
            {
                var helper = sp.GetRequiredService<ConfigHelper>();

                var version = helper.GetRequired<string>(
                    ConfigKeys.ScenariosSection,
                    ConfigKeys.EventsTests,
                    ConfigKeys.Version);

                client.BaseAddress = Factory.Server.BaseAddress;
                client.Timeout = TimeSpan.FromSeconds(90);
            }
            ).ConfigurePrimaryHttpMessageHandler(() =>
            {
                return Factory.Server.CreateHandler();
            });
        }

        protected T GetService<T>() where T : notnull
        {
            return serviceProvider.GetRequiredService<T>();
        }

        [TearDown]
        public async Task TearDown()
        {
            await serviceProvider.DisposeAsync();
            await Factory.DisposeAsync();
        }
    }
}