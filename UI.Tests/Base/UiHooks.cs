using Reqnroll;
using UI.Apps.SauceDemo;
using UI.Apps.SauceDemo.Auth;
using UI.Framework.Base;

namespace UI.Tests.Base
{
    [Binding]
    public sealed class UiHooks(BrowserSession browserSession, ISauceDemoAuthProvider authBootstrapper, SauceDemoOptions sauceDemoOptions, ScenarioContext scenarioContext)
    {
        [BeforeScenario("@ui")]
        public async Task BeforeUiScenarioAsync()
        {
            var userType = ResolveUserType();
            string? authProfile = null;

            if (userType is not null)
            {
                var user = sauceDemoOptions.GetUser(userType.Value);

                if (user.SupportsSavedAuthState)
                {
                    await authBootstrapper.EnsureAuthStateAsync(userType.Value);
                    authProfile = user.ProfileName;
                }
            }

            await browserSession.CreateContextAsync(authProfile);

            await browserSession.StartTracingAsync();
        }

        [AfterScenario("@ui")]
        public async Task AfterUiScenarioAsync()
        {
            var safeScenarioName = SanitizeFileName(scenarioContext.ScenarioInfo.Title);

            await browserSession.StopTracingAsync(safeScenarioName);
            await browserSession.DisposeAsync();
        }

        private SauceDemoUserType? ResolveUserType()
        {
            var tags = scenarioContext.ScenarioInfo.Tags;

            if (tags.Contains("auth-standard"))
                return SauceDemoUserType.Standard;

            if (tags.Contains("auth-visual"))
                return SauceDemoUserType.Visual;

            return null;
        }

        private static string SanitizeFileName(string value)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                value = value.Replace(invalidChar, '_');
            }

            return value.Replace(' ', '_');
        }
    }
}