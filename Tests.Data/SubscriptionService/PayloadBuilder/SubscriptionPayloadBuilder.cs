using Application.Contracts.SubscriptionContracts;
using System;

namespace Tests.Data.SubscriptionService.PayloadBuilder
{
    public class SubscriptionPayloadBuilder
    {
        private readonly string eventType = $"UserCreated-{Guid.NewGuid():N}";

        private readonly string webhookUrl = "http://wiremock:8080/webhooks/events";

        public SubscriptionDto Build()
        {
            return new SubscriptionDto
            {
                EventType = eventType,
                WebhookUrl = webhookUrl
            };
        }
    }
}