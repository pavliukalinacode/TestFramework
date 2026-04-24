using Microsoft.Playwright;
using Tests.Tools.Logger;

namespace UI.Framework.Base
{
    /// <summary>
    /// Owns the Playwright runtime and shared browser instance for the test run.
    /// The browser is started lazily and reused across scenarios.
    /// </summary>
    public sealed class PlaywrightBrowserHost(PlaywrightOptions options, ILog logger) : IDisposable, IAsyncDisposable
    {
        private readonly SemaphoreSlim syncLock = new(1, 1);
        private IPlaywright? playwright;
        private IBrowser? browser;
        private bool isStarted;
        private bool isDisposed;

        public async Task StartAsync()
        {
            ThrowIfDisposed();

            if (isStarted && browser is not null)
                return;

            await syncLock.WaitAsync();
            try
            {
                ThrowIfDisposed();

                if (isStarted && browser is not null)
                    return;

                playwright = await Playwright.CreateAsync();

                browser = options.NormalizedBrowserName switch
                {
                    "chromium" => await playwright.Chromium.LaunchAsync(options.CreateLaunchOptions()),
                    _ => throw new NotSupportedException($"Unsupported browser '{options.BrowserName}'.")
                };

                isStarted = true;
                logger.Info($"Playwright browser started: {options.BrowserName}, headless={options.Headless}");
            }
            finally
            {
                syncLock.Release();
            }
        }

        public async Task<IBrowser> GetBrowserAsync()
        {
            ThrowIfDisposed();

            await StartAsync();
            return browser ?? throw new InvalidOperationException("Browser was not initialized.");
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

            await syncLock.WaitAsync();
            try
            {
                if (isDisposed)
                    return;

                if (browser is not null)
                {
                    await browser.CloseAsync();
                    await browser.DisposeAsync();
                    browser = null;
                }

                playwright?.Dispose();
                playwright = null;

                isStarted = false;
                isDisposed = true;

                logger.Info("Playwright browser host disposed.");
            }
            finally
            {
                syncLock.Release();
                syncLock.Dispose();
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(PlaywrightBrowserHost));
        }
    }
}