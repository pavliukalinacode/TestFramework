using System.Net;

namespace Application.Contracts.EventContracts
{
    public class EventDeliveryResultDto
    {
        public string? WebhookUrl { get; set; }

        public HttpStatusCode? Status { get; set; }

        public string? Error { get; set; }
    }
}
