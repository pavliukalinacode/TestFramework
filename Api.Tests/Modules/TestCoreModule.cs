using Api.Tests.Base;
using Configuration.Config;
using Microsoft.Extensions.Configuration;
using Reqnroll.BoDi;
using Tests.Tools.Logger;

namespace Api.Tests.Modules
{
    public static class TestCoreModule
    {
        public static void Register(IObjectContainer container, IConfiguration config, string scenarioId)
        {
            var configHelper = new ConfigHelper(config);

            var loggerType = configHelper.GetRequiredString(
                ConfigKeys.GlobalParametersSection,
                ConfigKeys.LoggerSection,
                ConfigKeys.LoggerType);

            var logger = LoggerFactory.Create(loggerType);

            container.RegisterInstanceAs(config);
            container.RegisterInstanceAs(configHelper);
            container.RegisterInstanceAs<ILog>(logger);
            container.RegisterInstanceAs(new TestExecutionContext(config, scenarioId));
        }
    }
}