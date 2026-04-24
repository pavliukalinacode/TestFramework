using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tests.Tools.Logger;
using UI.Apps.SauceDemo.Flows;
using UI.Apps.SauceDemo.Pages;
using UI.Framework.Auth;
using UI.Framework.Base;

namespace UI.Apps.SauceDemo.Auth
{
    /// <summary>
    /// Ensures saved authentication state exists for SauceDemo users that support
    /// persistent auth profiles by performing a login flow and saving the resulting
    /// browser storage state.
    /// </summary>
    public sealed class SauceDemoAuthProvider(IAuthStateProvider authStateProvider, SauceDemoOptions sauceDemoOptions, PlaywrightBrowserHost browserHost, PlaywrightOptions playwrightOptions, ILog logger) : ISauceDemoAuthProvider, IDisposable
    {
        private readonly SemaphoreSlim syncLock = new(1, 1);
        private bool isDisposed;

        public async Task EnsureAuthStateAsync(SauceDemoUserType userType)
        {
            ThrowIfDisposed();

            var user = sauceDemoOptions.GetUser(userType);

            if (!user.SupportsSavedAuthState)
            {
                throw new InvalidOperationException(
                    $"User '{user.Type}' does not support saved auth state.");
            }

            if (string.IsNullOrWhiteSpace(user.ProfileName))
            {
                throw new InvalidOperationException(
                    $"User '{user.Type}' must define a profile name when saved auth state is enabled.");
            }

            await syncLock.WaitAsync();

            try
            {
                var existingPath = await authStateProvider.GetStorageStatePathAsync(user.ProfileName);
                if (AuthStateExists(existingPath))
                {
                    return;
                }

                var storageStatePath = BuildStorageStatePath(user.ProfileName);

                await using var session = new BrowserSession(browserHost, authStateProvider, playwrightOptions,logger);

                await session.CreateContextAsync();

                var loginPage = new LoginPage(session, sauceDemoOptions);
                var loginFlow = new SauceDemoLoginFlow(sauceDemoOptions, loginPage);

                await loginFlow.LoginAndWaitForSuccessAsync(userType);
                await session.SaveStorageStateAsync(storageStatePath);

                await authStateProvider.SaveStorageStatePathAsync(user.ProfileName, storageStatePath);
            }
            finally
            {
                syncLock.Release();
            }
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            syncLock.Dispose();
            isDisposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(SauceDemoAuthProvider));
        }

        private static bool AuthStateExists(string? path) =>
            !string.IsNullOrWhiteSpace(path) && File.Exists(path);

        private static string BuildStorageStatePath(string profileName)
        {
            var safeProfileName = profileName.Trim();
            var path = Path.Combine("artifacts", "auth", $"{safeProfileName}.json");

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return path;
        }
    }
}