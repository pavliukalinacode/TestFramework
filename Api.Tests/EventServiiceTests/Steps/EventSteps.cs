using Api.Services.Components;
using Api.Services.Models;
using Application.Contracts.EventContracts;
using Application.Contracts.SubscriptionContracts;
using Reqnroll;
using System.Threading.Tasks;
using Tests.Data.EventService.PayloadBuilder;

namespace Api.Tests.EventsServiceTests.Steps
{
    [Binding]
    public sealed class EventSteps(ScenarioContext scenarioContext, EventsService eventsService)
    {
        [Given(@"I have an event for the created subscription")]
        public void GivenIHaveAnEventForTheCreatedSubscription()
        {
            var subscription = scenarioContext.Get<SubscriptionDto>();

            var payload = new EventPayloadBuilder()
                .SetEventType(subscription.EventType)
                .Build();

            scenarioContext.Set(payload);
        }

        [When(@"I publish the event")]
        public async Task WhenIPublishTheEvent()
        {
            var payload = scenarioContext.Get<EventDto>();

            var response = await eventsService.PublishEvent<PublishEventResponseDto>(payload);

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [Then(@"the event publish response should show '(.*)' subscriber")]
        [Then(@"the event publish response should show '(.*)' subscribers")]
        public void ThenTheEventPublishResponseShouldShowSubscribers(int expectedCount)
        {
            var response = scenarioContext.Get<ApiResponse<PublishEventResponseDto>>();

            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.SubscribersCount, Is.EqualTo(expectedCount));
        }

        [Then(@"the event publish message should be ""(.*)""")]
        public void ThenTheEventPublishMessageShouldBe(string expectedMessage)
        {
            var response = scenarioContext.Get<ApiResponse<PublishEventResponseDto>>();

            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Message, Is.EqualTo(expectedMessage));
        }
    }
}