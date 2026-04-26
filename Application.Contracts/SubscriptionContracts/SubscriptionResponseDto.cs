namespace Application.Contracts.SubscriptionContracts
{
    public class SubscriptionResponseDto
    {
        public string EventType { get; set; } = default!;

        public string WebhookUrl { get; set; } = default!;
    }
}
