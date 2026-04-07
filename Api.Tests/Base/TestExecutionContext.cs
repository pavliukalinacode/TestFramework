using Microsoft.Extensions.Configuration;
using System;

namespace Api.Tests.Base
{
    /// <summary>
    /// Represents the execution context of a test scenario.
    /// Stores metadata such as scenario identifier and runtime-related information.
    /// </summary>
    public sealed class TestExecutionContext
    {
        public IConfiguration Config { get; }
        public string ScenarioId { get; }

        public TestExecutionContext(IConfiguration config, string scenarioId)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            ScenarioId = !string.IsNullOrWhiteSpace(scenarioId)
                ? scenarioId
                : throw new ArgumentException("ScenarioId cannot be null or empty.", nameof(scenarioId));
        }
    }
}