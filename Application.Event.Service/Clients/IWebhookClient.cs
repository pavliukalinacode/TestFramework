using Application.Contracts.EventContracts;

namespace Application.Event.Service.Clients
{
    public interface IWebhookClient
    {
        Task<HttpResponseMessage> SendAsync(
            string webhookUrl,
            EventDto evt,
            CancellationToken cancellationToken);
    }
}
