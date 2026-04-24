using System;
using System.Threading.Tasks;
using UI.Apps.SauceDemo.Auth;
using UI.Apps.SauceDemo.Pages;

namespace UI.Apps.SauceDemo.Flows
{
    public sealed class SauceDemoLoginFlow(
        SauceDemoOptions options,
        LoginPage loginPage)
    {
        private readonly SauceDemoOptions options = options ?? throw new ArgumentNullException(nameof(options));
        private readonly LoginPage loginPage = loginPage ?? throw new ArgumentNullException(nameof(loginPage));

        public async Task LoginAsync(SauceDemoUserType userType)
        {
            var user = GetUser(userType);

            await loginPage.OpenAsync();
            await loginPage.EnterUserNameAsync(user.UserName!);
            await loginPage.EnterPasswordAsync(user.Password!);
            await loginPage.SubmitAsync();
        }

        public async Task LoginAndWaitForSuccessAsync(SauceDemoUserType userType)
        {
            await LoginAsync(userType);
            await loginPage.WaitForSuccessfulLoginAsync();
        }

        private SauceDemoUser GetUser(SauceDemoUserType userType) =>
            options.GetUser(userType);
    }
}