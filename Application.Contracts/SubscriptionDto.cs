namespace Application.Contracts
{
    public class SubscriptionDto
    {
        public string EventType { get; set; } = default!;
        public string WebhookUrl { get; set; } = default!;
    }
}
