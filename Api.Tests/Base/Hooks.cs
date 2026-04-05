using Api.Tests.Modules;
using Reqnroll.BoDi;
using Configuration.Config;
using Microsoft.Extensions.Configuration;
using Reqnroll;
using System.Linq;
using System.Net.Http;

namespace Api.Tests.Base
{
    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer container;
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        public Hooks(
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
            if (scenarioContext.ScenarioInfo.Tags.Any())
                return scenarioContext.ScenarioInfo.Tags[0];

            if (featureContext.FeatureInfo.Tags.Any())
                return featureContext.FeatureInfo.Tags[0];

            return featureContext.FeatureInfo.Title.Replace(" ", "");
        }
    }
}