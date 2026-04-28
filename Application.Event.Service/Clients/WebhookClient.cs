using Application.Contracts.EventContracts;
using System.Text;
using System.Text.Json;

namespace Application.Event.Service.Clients
{
    public class WebhookClient(HttpClient httpClient) : IWebhookClient
    {
        public async Task<HttpResponseMessage> SendAsync(
            string webhookUrl,
            EventDto evt,
            CancellationToken cancellationToken)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(evt),
                Encoding.UTF8,
                "application/json");

            return await httpClient.PostAsync(
                webhookUrl,
                content,
                cancellationToken);
        }
    }
}
