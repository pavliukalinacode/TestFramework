using Api.Services.Components;
using Api.Services.Models;
using Application.Contracts.SubscriptionContracts;
using Reqnroll;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Data.SubscriptionService.PayloadBuilder;

namespace Api.Tests.SubscriptionServiceTests.Steps
{
    [Binding]
    public sealed class SubscriptionSteps(
        ScenarioContext scenarioContext,
        SubscriptionService subscriptionService)
    {
        [Given(@"I have a valid subscription")]
        public void GivenIHaveAValidSubscription()
        {
            var payload = new SubscriptionPayloadBuilder().Build();

            scenarioContext.Set(payload);
        }

        [Given(@"I create the subscription")]
        [When(@"I create the subscription")]
        public async Task WhenICreateTheSubscription()
        {
            var payload = scenarioContext.Get<SubscriptionDto>();

            var response =
                await subscriptionService.CreateSubscription<SubscriptionResponseDto>(payload);

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I retrieve subscriptions by submitted event type")]
        public async Task WhenIRetrieveSubscriptionsBySubmittedEventType()
        {
            var payload = scenarioContext.Get<SubscriptionDto>();

            var response =
                await subscriptionService.GetSubscriptionsByEventType<List<SubscriptionResponseDto>>(payload.EventType);

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [Then(@"the created subscription matches the submitted payload")]
        public void ThenTheCreatedSubscriptionMatchesTheSubmittedPayload()
        {
            var payload = scenarioContext.Get<SubscriptionDto>();
            var response = scenarioContext.Get<ApiResponse<SubscriptionResponseDto>>();

            Assert.That(response.Data, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(response.Data!.EventType, Is.EqualTo(payload.EventType));
                Assert.That(response.Data.WebhookUrl, Is.EqualTo(payload.WebhookUrl));
            });
        }

        [Then(@"the retrieved subscriptions contain the submitted subscription")]
        public void ThenTheRetrievedSubscriptionsContainTheSubmittedSubscription()
        {
            var payload = scenarioContext.Get<SubscriptionDto>();
            var response = scenarioContext.Get<ApiResponse<List<SubscriptionResponseDto>>>();

            Assert.That(response.Data, Is.Not.Null);

            Assert.That(response.Data!, Has.Some.Matches<SubscriptionResponseDto>(x =>
                x.EventType == payload.EventType &&
                x.WebhookUrl == payload.WebhookUrl));
        }
    }
}