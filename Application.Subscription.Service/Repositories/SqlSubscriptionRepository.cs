using Application.Subscription.Service.Data;
using Application.Subscription.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Subscription.Service.Repositories
{
    public class SqlSubscriptionRepository(NotificationDbContext dbContext) : ISubscriptionRepository
    {
        private readonly NotificationDbContext _dbContext = dbContext;

        public async Task AddAsync(SubscriptionEntity subscription, CancellationToken cancellationToken = default)
        {
            _dbContext.Subscriptions.Add(subscription);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<SubscriptionEntity>> GetByEventTypeAsync(
            string eventType,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Subscriptions
                .Where(x => x.EventType == eventType)
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
