using Api.Services.Components;
using Application.Contracts.EventContracts;
using Application.Contracts.SubscriptionContracts;
using Component.Integration.Tests.Base;
using FluentAssertions;
using Moq;
using System.Net;
using System.Text;
using Tests.Data.EventService.PayloadBuilder;

namespace Component.Integration.Tests.Tests.EventTests
{
    [TestFixture]
    public class EventComponentTests : ComponentTestBase
    {
        [Test]
        public async Task PublishEvent_WhenSubscriptionExists_ShouldCallSubscriptionAndWebhookClients()
        {
            var eventsService = GetService<EventsService>();
            var eventType = "UserCreated";
            var webhookUrl = "https://test.com/webhook";

            Factory.SubscriptionMock
                .Setup(x => x.GetSubscriptionsAsync(eventType, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<SubscriptionDto>
                {
                new()
                {
                    EventType = eventType,
                    WebhookUrl = webhookUrl
                }
                });

            Factory.WebhookMock
                .Setup(x => x.SendAsync(
                    webhookUrl,
                    It.IsAny<EventDto>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var evt = new EventPayloadBuilder()
                .SetEventType(eventType)
                .Build();

            var response = await eventsService.PublishEvent<object>(evt);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            Factory.SubscriptionMock.Verify(
                x => x.GetSubscriptionsAsync(eventType, It.IsAny<CancellationToken>()),
                Times.Once);

            Factory.WebhookMock.Verify(
                x => x.SendAsync(
                    webhookUrl,
                    It.Is<EventDto>(e =>
                        e.EventType == evt.EventType &&
                        e.Payload == evt.Payload),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
