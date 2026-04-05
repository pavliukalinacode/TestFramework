using Microsoft.Extensions.Configuration;
using System;

namespace Configuration.Config
{
    public class ConfigHelper(IConfiguration config)
    {
        private readonly IConfiguration config = config ?? throw new ArgumentNullException(nameof(config));

        public string GetRequiredString(params string[] pathParts)
        {
            var key = BuildKey(pathParts);
            var value = config[key];

            return !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new InvalidOperationException($"Missing configuration value for '{key}'.");
        }

        public int GetRequiredInt(params string[] pathParts)
        {
            var key = BuildKey(pathParts);
            var value = config.GetValue<int?>(key);

            return value
                ?? throw new InvalidOperationException($"Missing configuration value for '{key}'.");
        }

        private static string BuildKey(params string[] parts)
        {
            return string.Join(":", parts);
        }
    }
}