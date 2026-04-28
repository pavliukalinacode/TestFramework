using Application.Contracts.SubscriptionContracts;
using Application.Event.Service;
using Application.Event.Service.Clients;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Component.Integration.Tests.Base
{
    public class EventServiceFactory : WebApplicationFactory<Program>
    {
        public Mock<ISubscriptionClient> SubscriptionMock { get; } = new();
        public Mock<IWebhookClient> WebhookMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("ComponentTest");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<ISubscriptionClient>();
                services.AddSingleton(SubscriptionMock.Object);

                services.RemoveAll<IWebhookClient>();
                services.AddSingleton(WebhookMock.Object);
            });
        }
    }
}
