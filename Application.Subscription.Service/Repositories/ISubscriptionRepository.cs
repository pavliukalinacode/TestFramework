using Application.Subscription.Service.Models;

namespace Application.Subscription.Service.Repositories
{
    public interface ISubscriptionRepository
    {
        Task AddAsync(SubscriptionEntity subscription, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SubscriptionEntity>> GetByEventTypeAsync(string eventType, CancellationToken cancellationToken = default);
    }
}
