using Application.Contracts.SubscriptionContracts;

namespace Application.Event.Service.Clients
{
    public interface ISubscriptionClient
    {
        Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(
            string eventType,
            CancellationToken cancellationToken = default);
    }
}
