using Api.Tests.Modules;
using Configuration.Config;
using Microsoft.Extensions.Configuration;
using Reqnroll;
using Reqnroll.BoDi;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Api.Tests.Base.Hooks
{
    [Binding]
    public sealed class BaseHooks
    {
        private readonly IObjectContainer container;
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        private static readonly Regex Sanitizer = new(@"[^a-zA-Z0-9]+", RegexOptions.Compiled);

        public BaseHooks(
            IObjectContainer container,
            FeatureContext featureContext,
            ScenarioContext scenarioContext)
        {
            this.container = container;
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            IConfiguration config = new Configurator().GetConfig();

            var scenarioId = ResolveScenarioId();

            TestCoreModule.Register(container, config, scenarioId);

            var configHelper = container.Resolve<ConfigHelper>();

            PetModule.Register(container, configHelper);
        }

        [AfterScenario(Order = 100)]
        public void AfterScenario()
        {
            if (container.IsRegistered<HttpClient>())
            {
                var httpClient = container.Resolve<HttpClient>();
                httpClient.Dispose();
            }
        }

        private string ResolveScenarioId()
        {
            var tags = scenarioContext.ScenarioInfo.Tags
                .Concat(featureContext.FeatureInfo.Tags)
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Select(SanitizeTag)
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (tags.Length > 0)
            {
                return string.Join("_", tags);
            }

            var fallback = $"{featureContext.FeatureInfo.Title}_{scenarioContext.ScenarioInfo.Title}";
            return SanitizeTag(fallback);
        }

        private static string SanitizeTag(string value)
        {
            return Sanitizer
                .Replace(value.Trim().TrimStart('@'), "_")
                .Trim('_');
        }
    }
}