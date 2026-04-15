using Microsoft.Playwright;
using UI.Framework.Auth;

namespace UI.Framework.Base
{
    public sealed class BrowserSession(PlaywrightBrowserHost browserHost, PlaywrightOptions options, IAuthStateProvider authStateProvider) : IDisposable, IAsyncDisposable
    {
        private IBrowserContext? context;
        private IPage? page;

        public IBrowserContext Context => context ?? throw SessionNotInitialized("browser context");
        public IPage Page => page ?? throw SessionNotInitialized("page");

        public async Task CreateContextAsync(string? authProfile = null)
        {
            await ResetAsync();

            var browser = await browserHost.GetBrowserAsync();
            var storageStatePath = await authStateProvider.GetStorageStatePathAsync(authProfile);

            context = await browser.NewContextAsync(options.CreateContextOptions(storageStatePath));
            options.ApplyTimeouts(context);

            page = await context.NewPageAsync();
        }

        public async Task SaveStorageStateAsync(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);

            if (Path.GetDirectoryName(path) is { } dir)
                Directory.CreateDirectory(dir);

            await Context.StorageStateAsync(new() { Path = path });
        }

        public async Task StartTracingAsync()
        {
            if (!options.Tracing.Enabled || context is null)
            {
                return;
            }

            await context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = options.Tracing.Screenshots,
                Snapshots = options.Tracing.Snapshots,
                Sources = options.Tracing.Sources
            });
        }

        public async Task StopTracingAsync(string traceName)
        {
            if (!options.Tracing.Enabled || context is null)
            {
                return;
            }

            Directory.CreateDirectory(options.Tracing.OutputFolder);

            var path = Path.Combine(
                options.Tracing.OutputFolder,
                $"{options.Tracing.TraceNamePrefix}-{traceName}.zip");

            await context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = path
            });
        }

        public async Task ResetAsync()
        {
            if (context is null)
            {
                page = null;
                return;
            }

            await context.DisposeAsync();
            context = null;
            page = null;
        }

        public void Dispose()
        {
            if (context is not null)
            {
                context.DisposeAsync().AsTask().GetAwaiter().GetResult();
                context = null;
            }

            page = null;
        }

        public ValueTask DisposeAsync() => new(ResetAsync());

        private static InvalidOperationException SessionNotInitialized(string resource) =>
            new($"The {resource} has not been created. Call {nameof(CreateContextAsync)} first.");
    }
}