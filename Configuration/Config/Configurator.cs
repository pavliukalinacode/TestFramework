using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Configuration.Config
{
    public class Configurator
    {
        private const string DefaultSettingsFilename = "appsettings";
        private const string EnvironmentVariableName = "CLOUD";

        public IConfiguration GetConfig(string? environment = null)
        {
            var env = environment ?? Environment.GetEnvironmentVariable(EnvironmentVariableName);

            var fileName = string.IsNullOrWhiteSpace(env)
                ? $"{DefaultSettingsFilename}.Local.json"
                : $"{DefaultSettingsFilename}.{env}.json";

            var basePath = Path.GetDirectoryName(typeof(Configurator).Assembly.Location)!;

            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(fileName, optional: false, reloadOnChange: false)
                .Build();
        }
    }
}