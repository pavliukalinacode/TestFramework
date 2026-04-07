using Api.Services.Components;
using Configuration.Config;
using Reqnroll.BoDi;
using System;
using System.Net.Http;

namespace Api.Tests.Modules
{
    /// <summary>
    /// Registers Pet API-related dependencies.
    /// Provides PetService and supporting components to the test container.
    /// </summary>
    public static class PetModule
    {
        public static void Register(IObjectContainer container, ConfigHelper configHelper)
        {
            var baseUrl = configHelper.GetRequiredString(
                ConfigKeys.ScenariosSection,
                ConfigKeys.PetTests,
                ConfigKeys.BaseUrl);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            container.RegisterInstanceAs(httpClient);
            container.RegisterTypeAs<PetService, PetService>();
        }
    }
}