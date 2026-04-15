using Reqnroll;
using System;
using System.Linq;
using Tests.Tools.Logger;

namespace Api.Tests.Base
{
    [Binding]
    public sealed class Hooks(ILog logger, ScenarioContext scenarioContext)
    {
        [BeforeScenario("@quarantined", Order = 10)]
        public void SkipQuarantinedScenario()
        {
            var additionalTags = scenarioContext.ScenarioInfo.Tags
                .Where(tag => !string.Equals(tag, "quarantined", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            var tagInfo = additionalTags.Length > 0
                ? string.Join(", ", additionalTags)
                : "no additional tags";

            logger.Info($"Skipping quarantined scenario. Additional tags: [{tagInfo}]");

            Assert.Ignore($"Scenario is quarantined and skipped by design. Additional tags: [{tagInfo}]");
        }
    }
}