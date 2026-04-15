using Microsoft.Playwright;

namespace UI.Framework.Base
{
    public sealed class BrowserFactory(BrowserTypeProvider browserTypeProvider)
    {
        public async Task<IBrowser> LaunchAsync(IPlaywright playwright, PlaywrightOptions options)
        {
            ArgumentNullException.ThrowIfNull(playwright);
            ArgumentNullException.ThrowIfNull(options);

            var browserType = browserTypeProvider.Get(playwright, options.NormalizedBrowserName);

            return await browserType.LaunchAsync(options.CreateLaunchOptions());
        }
    }
}