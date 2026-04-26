namespace Application.Subscription.Service.Models
{
    public class SubscriptionEntity
    {
        public int Id { get; set; }
        public string EventType { get; set; } = default!;
        public string WebhookUrl { get; set; } = default!;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
