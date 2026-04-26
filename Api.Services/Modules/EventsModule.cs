using Api.Services.Components;
using Configuration.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Services.Modules
{
    /// <summary>
    /// Registers Events API-related dependencies.
    /// Provides EventsService and supporting components to the test container.
    /// </summary>
    public static class EventsModule
    {
        public static IServiceCollection AddEventsModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddHttpClient<EventsService>((sp, client) =>
            {
                var helper = sp.GetRequiredService<ConfigHelper>();
                var baseUrl = helper.GetRequired<string>(
                    ConfigKeys.ScenariosSection,
                    ConfigKeys.EventsTests,
                    ConfigKeys.BaseUrl);

                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(90);
            });

            return services;
        }
    }
}
