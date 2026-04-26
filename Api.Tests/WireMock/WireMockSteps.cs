using Api.Services.Components;
using Application.Contracts.EventContracts;
using Newtonsoft.Json.Linq;
using Reqnroll;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Tests.WireMock
{
    [Binding]
    public sealed class WireMockSteps(ScenarioContext scenarioContext, WireMockService wireMockService)
    {
        [Given("WireMock requests are reset")]
        public async Task GivenWireMockRequestsAreReset()
        {
            var response = await wireMockService.ResetRequests();

            scenarioContext.Set((int)response.StatusCode);
        }

        [Then("the webhook should receive the event")]
        public async Task ThenTheWebhookShouldReceiveTheEvent()
        {
            var expectedEvent = scenarioContext.Get<EventDto>();

            var response = await wireMockService.GetRequests();

            Assert.That(response.Content, Is.Not.Null.And.Not.Empty);

            var json = JObject.Parse(response.Content!);
            var requests = json["requests"] as JArray;

            Assert.That(requests, Is.Not.Null);
            Assert.That(requests!.Count, Is.GreaterThan(0));

            var hasWebhookEvent = requests.Any(request =>
            {
                var url = request["request"]?["url"]?.ToString();
                var method = request["request"]?["method"]?.ToString();
                var body = request["request"]?["body"]?.ToString();

                if (url != "/webhooks/events" || method != "POST" || string.IsNullOrWhiteSpace(body))
                {
                    return false;
                }

                var bodyJson = JObject.Parse(body);

                var actualEventType = bodyJson["EventType"]?.ToString()
                    ?? bodyJson["eventType"]?.ToString();

                var actualPayload = bodyJson["Payload"]?.ToString()
                    ?? bodyJson["payload"]?.ToString();

                return actualEventType == expectedEvent.EventType
                    && actualPayload == expectedEvent.Payload;
            });

            Assert.That(hasWebhookEvent, Is.True,
                $"Expected WireMock to receive event '{expectedEvent.EventType}'.");
        }
    }
}
