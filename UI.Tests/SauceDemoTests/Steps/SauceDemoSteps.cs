using NUnit.Framework;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Text;
using UI.Apps.SauceDemo.Auth;
using UI.Apps.SauceDemo.Flows;
using UI.Apps.SauceDemo.Pages;

namespace UI.Tests.SauceDemoTests.Steps
{
    [Binding]
    public sealed class SauceDemoLoginSteps(SauceDemoLoginFlow loginFlow, LoginPage loginPage, InventoryPage inventoryPage)
    {
        [When("I open the Sauce Demo login page")]
        public async Task WhenIOpenTheSauceDemoLoginPage()
        {
            await loginPage.OpenAsync();
        }

        [When("I log in as the standard user")]
        public async Task WhenILogInAsTheStandardUser()
        {
            await loginFlow.LoginAsync(SauceDemoUserType.Standard);
        }

        [When("I log in as the visual user")]
        public async Task WhenILogInAsTheVisualUser()
        {
            await loginFlow.LoginAsync(SauceDemoUserType.Visual);
        }

        [When("I open the inventory page")]
        public async Task WhenIOpenTheInventoryPage()
        {
            await inventoryPage.OpenAsync();
        }

        [Then("I should be on the inventory page")]
        public async Task ThenIShouldBeOnTheInventoryPage()
        {
            Assert.That(await inventoryPage.IsOpenAsync(), Is.True);
        }

        [Then("the inventory title should be {string}")]
        public async Task ThenTheInventoryTitleShouldBe(string expectedTitle)
        {
            var actualTitle = await inventoryPage.GetTitleAsync();
            Assert.That(actualTitle, Is.EqualTo(expectedTitle));
        }
    }
}
