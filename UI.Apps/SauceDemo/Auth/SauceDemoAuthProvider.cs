using Microsoft.Playwright;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UI.Apps.SauceDemo.Flows;
using UI.Apps.SauceDemo.Pages;
using UI.Framework.Auth;
using UI.Framework.Base;

namespace UI.Apps.SauceDemo.Auth;

public sealed class SauceDemoAuthProvider(
    BrowserSessionFactory browserSessionFactory,
    IAuthStateProvider authStateProvider,
    SauceDemoOptions sauceDemoOptions) : ISauceDemoAuthProvider
{
    private readonly SemaphoreSlim syncLock = new(1, 1);

    public async Task EnsureAuthStateAsync(SauceDemoUserType userType)
    {
        var user = sauceDemoOptions.GetUser(userType);

        if (!user.SupportsSavedAuthState)
        {
            throw new InvalidOperationException(
                $"User '{user.Type}' does not support saved auth state.");
        }

        await syncLock.WaitAsync();

        try
        {
            var existingPath = await authStateProvider.GetStorageStatePathAsync(user.ProfileName);
            if (AuthStateExists(existingPath))
            {
                return;
            }

            var storageStatePath = BuildStorageStatePath(user.ProfileName!);

            await using var session = await browserSessionFactory.CreateAsync();

            var loginPage = new LoginPage(session, sauceDemoOptions);
            var loginFlow = new SauceDemoLoginFlow(sauceDemoOptions, loginPage);

            await loginFlow.LoginAndWaitForSuccessAsync(userType);
            await session.SaveStorageStateAsync(storageStatePath);

            await authStateProvider.SaveStorageStatePathAsync(user.ProfileName!, storageStatePath);
        }
        finally
        {
            syncLock.Release();
        }
    }

    private static bool AuthStateExists(string? path) =>
        !string.IsNullOrWhiteSpace(path) && File.Exists(path);

    private static string BuildStorageStatePath(string profileName)
    {
        var path = Path.Combine("artifacts", "auth", $"{profileName}.json");

        if (Path.GetDirectoryName(path) is { } dir)
        {
            Directory.CreateDirectory(dir);
        }

        return path;
    }
}