using Microsoft.Playwright;
using System.Threading.Tasks;
using UI.Framework.Base;

namespace UI.Apps.SauceDemo.Pages
{
    public sealed class LoginPage(BrowserSession browserSession, SauceDemoOptions options)
    {
        private ILocator UserNameInput => browserSession.Page.Locator("[data-test='username']");
        private ILocator PasswordInput => browserSession.Page.Locator("[data-test='password']");
        private ILocator LoginButton => browserSession.Page.Locator("[data-test='login-button']");

        public Task OpenAsync() => browserSession.Page.GotoAsync(options.BaseUrl);

        public Task EnterUserNameAsync(string userName) => UserNameInput.FillAsync(userName);

        public Task EnterPasswordAsync(string password) => PasswordInput.FillAsync(password);

        public Task SubmitAsync() => LoginButton.ClickAsync();

        public Task WaitForSuccessfulLoginAsync() => browserSession.Page.WaitForURLAsync("**/inventory.html");
    }
}