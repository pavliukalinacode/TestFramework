using Microsoft.Extensions.Configuration;
using System;

namespace Configuration.Config
{
    /// <summary>
    /// Provides helper methods for accessing configuration values.
    /// Enables strongly-typed retrieval of settings.
    /// </summary>
    public class ConfigHelper(IConfiguration config)
    {
        private readonly IConfiguration config = config ?? throw new ArgumentNullException(nameof(config));

        public T GetRequired<T>(params string[] pathParts)
        {
            var key = BuildKey(pathParts);
            var value = config.GetValue<T?>(key);

            if (value is null)
            {
                throw new InvalidOperationException($"Missing configuration value for '{key}'.");
            }

            return value;
        }

        private static string BuildKey(params string[] parts)
        {
            return string.Join(":", parts);
        }
    }
}