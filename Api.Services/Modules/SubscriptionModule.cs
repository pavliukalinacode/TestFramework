using Api.Services.Components;
using Configuration.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Services.Modules
{
    /// <summary>
    /// Registers Subscription API-related dependencies.
    /// Provides SubscriptionService and supporting components to the test container.
    /// </summary>
    public static class SubscriptionModule
    {
        public static IServiceCollection AddSubscriptionModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddHttpClient<SubscriptionService>((sp, client) =>
            {
                var helper = sp.GetRequiredService<ConfigHelper>();
                var baseUrl = helper.GetRequired<string>(
                    ConfigKeys.ScenariosSection,
                    ConfigKeys.SubscriptionTests,
                    ConfigKeys.BaseUrl);

                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(90);
            });

            return services;
        }
    }
}
