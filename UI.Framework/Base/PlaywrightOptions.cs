using Microsoft.Playwright;

namespace UI.Framework.Base
{
    public sealed class PlaywrightOptions
    {
        public string BrowserName { get; init; } = "chromium";
        public bool Headless { get; init; } = true;
        public int SlowMoMs { get; init; }
        public int ExpectTimeoutMs { get; init; } = 5000;

        public PlaywrightTracingOptions Tracing { get; init; } = new();

        public string NormalizedBrowserName => BrowserName.Trim().ToLowerInvariant();

        public BrowserTypeLaunchOptions CreateLaunchOptions() => new()
        {
            Headless = Headless,
            SlowMo = SlowMoMs > 0 ? SlowMoMs : null
        };

        public BrowserNewContextOptions CreateContextOptions(string? storageStatePath = null)
        {
            var options = new BrowserNewContextOptions();

            if (!string.IsNullOrWhiteSpace(storageStatePath))
            {
                options.StorageStatePath = storageStatePath;
            }

            return options;
        }

        public void ApplyTimeouts(IBrowserContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            context.SetDefaultTimeout(ExpectTimeoutMs);
            context.SetDefaultNavigationTimeout(ExpectTimeoutMs);
        }
    }

    public sealed class PlaywrightTracingOptions
    {
        public bool Enabled { get; init; }
        public bool Screenshots { get; init; }
        public bool Snapshots { get; init; }
        public bool Sources { get; init; }
        public string TraceNamePrefix { get; init; } = "trace";
        public string OutputFolder { get; init; } = "artifacts/traces";
    }
}