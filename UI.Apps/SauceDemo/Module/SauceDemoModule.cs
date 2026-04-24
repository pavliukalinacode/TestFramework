using Configuration.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using UI.Apps.SauceDemo.Auth;
using UI.Apps.SauceDemo.Flows;
using UI.Apps.SauceDemo.Pages;

namespace UI.Apps.SauceDemo.Module
{
    public static class SauceDemoModule
    {
        public static IServiceCollection AddSauceDemoModule(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton(sp =>
            {
                var configHelper = sp.GetRequiredService<ConfigHelper>();

                return configHelper.GetRequiredSection<SauceDemoOptions>(
                    ConfigKeys.ScenariosSection,
                    ConfigKeys.SauceDemoTests);
            });

            services.AddScoped<LoginPage>();
            services.AddScoped<InventoryPage>();
            services.AddScoped<SauceDemoLoginFlow>();

            services.AddSingleton<ISauceDemoAuthProvider, SauceDemoAuthProvider>();

            return services;
        }
    }
}