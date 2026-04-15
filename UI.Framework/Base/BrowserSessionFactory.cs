using UI.Framework.Auth;

namespace UI.Framework.Base
{
    public sealed class BrowserSessionFactory(PlaywrightBrowserHost browserHost, PlaywrightOptions options, IAuthStateProvider authStateProvider)
    {
        public async Task<BrowserSession> CreateAsync(string? authProfile = null)
        {
            var session = new BrowserSession(browserHost, options, authStateProvider);
            await session.CreateContextAsync(authProfile);
            return session;
        }
    }
}
