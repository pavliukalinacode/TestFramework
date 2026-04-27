using Application.Contracts.SubscriptionContracts;
using System.Text.Json;

namespace Application.Event.Service.Clients
{
    public class SubscriptionClient(HttpClient httpClient) : ISubscriptionClient
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task<IReadOnlyList<SubscriptionDto>> GetSubscriptionsAsync(
            string eventType,
            CancellationToken cancellationToken = default)
        {
            var response = await httpClient.GetAsync(
                $"/v1/subscriptions/{eventType}",
                cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.Deserialize<List<SubscriptionDto>>(
                       json,
                       new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? [];
        }
    }
}