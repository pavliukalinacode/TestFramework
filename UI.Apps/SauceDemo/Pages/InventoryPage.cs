using Microsoft.Playwright;
using System.Threading.Tasks;
using UI.Framework.Base;

namespace UI.Apps.SauceDemo.Pages
{
    public sealed class InventoryPage(BrowserSession browserSession, SauceDemoOptions options)
    {
        private ILocator InventoryContainer => browserSession.Page.Locator("[data-test='inventory-container']");
        private ILocator Title => browserSession.Page.Locator("[data-test='title']");

        public Task OpenAsync() =>
        browserSession.Page.GotoAsync($"{options.BaseUrl}/inventory.html");

        public Task<bool> IsOpenAsync() => InventoryContainer.IsVisibleAsync();

        public Task<string> GetTitleAsync() => Title.InnerTextAsync();
    }
}