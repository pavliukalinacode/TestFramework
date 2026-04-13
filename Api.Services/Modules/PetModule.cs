using Api.Services.Components;
using Configuration.Config;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api.Services.Modules
{
    /// <summary>
    /// Registers Pet API-related dependencies.
    /// Provides PetService and supporting components to the test container.
    /// </summary>
    public static class PetModule
    {
        public static IServiceCollection AddPetModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddHttpClient<PetService>((sp, client) =>
            {
                var helper = sp.GetRequiredService<ConfigHelper>();
                var baseUrl = helper.GetRequired<string>(ConfigKeys.ScenariosSection, ConfigKeys.PetTests, ConfigKeys.BaseUrl);

                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(90);
            });

            return services;
        }
    }
}