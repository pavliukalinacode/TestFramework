using Microsoft.Playwright;
using Tests.Tools.Logger;
using UI.Framework.Auth;

namespace UI.Framework.Base
{
    /// <summary>
    /// Owns the scenario-scoped browser context and page, and manages tracing
    /// for a single UI scenario.
    /// </summary>
    public sealed class BrowserSession : IDisposable, IAsyncDisposable
    {
        private readonly PlaywrightBrowserHost browserHost;
        private readonly IAuthStateProvider authStateProvider;
        private readonly PlaywrightOptions options;
        private readonly ILog logger;

        private IBrowserContext? context;
        private IPage? page;
        private bool traceStarted;
        private bool isDisposed;

        public BrowserSession(
            PlaywrightBrowserHost browserHost,
            IAuthStateProvider authStateProvider,
            PlaywrightOptions options,
            ILog logger)
        {
            this.browserHost = browserHost ?? throw new ArgumentNullException(nameof(browserHost));
            this.authStateProvider = authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IBrowserContext Context =>
            context ?? throw new InvalidOperationException("Browser session not initialized.");

        public IPage Page =>
            page ?? throw new InvalidOperationException("Browser session not initialized.");

        public bool IsInitialized => context is not null && page is not null;

        public async Task CreateContextAsync(string? authProfile = null)
        {
            ThrowIfDisposed();

            if (IsInitialized)
                return;

            var browser = await browserHost.GetBrowserAsync();

            string? storageStatePath = null;
            if (!string.IsNullOrWhiteSpace(authProfile))
            {
                storageStatePath = await authStateProvider.GetStorageStatePathAsync(authProfile);
            }

            context = await browser.NewContextAsync(options.CreateContextOptions(storageStatePath));
            options.ApplyTimeouts(context);

            page = await context.NewPageAsync();

            logger.Info($"Browser session created. Auth profile: {authProfile ?? "none"}");
        }

        public async Task StartTracingAsync()
        {
            ThrowIfDisposed();

            if (!options.Tracing.Enabled || context is null || traceStarted)
                return;

            await context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = options.Tracing.Screenshots,
                Snapshots = options.Tracing.Snapshots,
                Sources = options.Tracing.Sources
            });

            traceStarted = true;
            logger.Info("Playwright tracing started.");
        }

        public async Task StopTracingAsync(string traceName)
        {
            ThrowIfDisposed();

            if (!options.Tracing.Enabled || context is null || !traceStarted)
                return;

            Directory.CreateDirectory(options.Tracing.OutputFolder);

            var safeTraceName = string.IsNullOrWhiteSpace(traceName)
                ? $"{options.Tracing.TraceNamePrefix}_{DateTime.UtcNow:yyyyMMdd_HHmmss}"
                : $"{options.Tracing.TraceNamePrefix}_{traceName}";

            var tracePath = Path.Combine(options.Tracing.OutputFolder, $"{safeTraceName}.zip");

            await context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = tracePath
            });

            traceStarted = false;
            logger.Info($"Playwright trace saved: {tracePath}");
        }

        public async Task SaveStorageStateAsync(string path)
        {
            ThrowIfDisposed();

            if (context is null)
                throw new InvalidOperationException("Browser context is not initialized.");

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await context.StorageStateAsync(new BrowserContextStorageStateOptions
            {
                Path = path
            });
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            DisposeAsync().AsTask().GetAwaiter().GetResult();
        }

        public async ValueTask DisposeAsync()
        {
            if (isDisposed)
                return;

            try
            {
                if (traceStarted && context is not null)
                {
                    await StopTracingAsync("dispose_fallback");
                }

                if (page is not null)
                {
                    await page.CloseAsync();
                    page = null;
                }

                if (context is not null)
                {
                    await context.CloseAsync();
                    await context.DisposeAsync();
                    context = null;
                }

                traceStarted = false;
                logger.Info("Browser session disposed.");
            }
            finally
            {
                isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(BrowserSession));
        }
    }
}
