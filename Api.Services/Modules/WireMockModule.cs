using Api.Services.Components;
using Configuration.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Services.Modules
{
    public static class WireMockModule
    {
        public static IServiceCollection AddWireMockModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddHttpClient<WireMockService>((sp, client) =>
            {
                var helper = sp.GetRequiredService<ConfigHelper>();

                var baseUrl = helper.GetRequired<string>(
                    ConfigKeys.GlobalParametersSection,
                    ConfigKeys.WireMockSection,
                    ConfigKeys.BaseUrl);

                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}
