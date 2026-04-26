namespace Application.Contracts.SubscriptionContracts
{
    public class SubscriptionDto
    {
        public string EventType { get; set; } = default!;
        public string WebhookUrl { get; set; } = default!;
    }
}
