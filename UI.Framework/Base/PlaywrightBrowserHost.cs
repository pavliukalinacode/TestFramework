using Microsoft.Playwright;

namespace UI.Framework.Base
{
    public sealed class PlaywrightBrowserHost(PlaywrightOptions options, BrowserFactory browserFactory) : IAsyncDisposable
    {
        private IPlaywright? playwright;
        private IBrowser? browser;

        public async Task<IBrowser> GetBrowserAsync()
        {
            if (browser is not null)
            {
                return browser;
            }

            playwright = await Playwright.CreateAsync();
            browser = await browserFactory.LaunchAsync(playwright, options);

            return browser;
        }

        public async ValueTask DisposeAsync()
        {
            if (browser is not null)
            {
                await browser.DisposeAsync();
            }

            playwright?.Dispose();

            browser = null;
            playwright = null;
        }
    }
}
