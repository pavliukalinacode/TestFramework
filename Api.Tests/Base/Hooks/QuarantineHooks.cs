using Reqnroll;
using System.Linq;
using Tests.Tools.Logger;

namespace Api.Tests.Base
{
    [Binding]
    public sealed class QuarantineHooks
    {
        private readonly ILog logger;
        private readonly ScenarioContext scenarioContext;

        public QuarantineHooks(ILog logger, ScenarioContext scenarioContext)
        {
            this.logger = logger;
            this.scenarioContext = scenarioContext;
        }

        [BeforeScenario("@quarantined", Order = 10)]
        public void SkipQuarantinedScenario()
        {
            var reasonTags = scenarioContext.ScenarioInfo.Tags
                .Where(tag => !string.Equals(tag, "quarantined", System.StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var reason = reasonTags.Length > 0
                ? string.Join(", ", reasonTags)
                : "no explicit reason tag provided";

            logger.Info($"Skipping quarantined scenario. Reason tags: [{reason}]");

            Assert.Ignore($"Scenario is quarantined and skipped by design. Reason tags: [{reason}]");
        }
    }
}