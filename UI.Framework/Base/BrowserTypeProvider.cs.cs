using Microsoft.Playwright;

namespace UI.Framework.Base
{
    public sealed class BrowserTypeProvider
    {
        public IBrowserType Get(IPlaywright playwright, string browserName)
        {
            ArgumentNullException.ThrowIfNull(playwright);

            return browserName.Trim().ToLowerInvariant() switch
            {
                "chromium" => playwright.Chromium,

                "firefox" => throw new NotSupportedException("Firefox not supported yet"),

                "webkit" => throw new NotSupportedException("WebKit not supported yet"),

                _ => throw new InvalidOperationException(
                    $"Unsupported browser '{browserName}'.")
            };
        }
    }
}