using Application.Contracts.EventContracts;
using Application.Contracts.SubscriptionContracts;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Application.Event.Service.Controllers
{
    [ApiController]
    [Route("v1/events")]
    public class EventController(HttpClient httpClient) : ControllerBase
    {
        private readonly HttpClient _httpClient = httpClient;

        // TODO: Move base URL to config or service discovery (K8s DNS)
        private const string SubscriptionServiceBaseUrl = "http://subscription-service:8080";

        [HttpPost]
        public async Task<IActionResult> Publish(EventDto evt)
        {
            // 1. Get subscribers
            var response = await _httpClient.GetAsync(
                $"{SubscriptionServiceBaseUrl}/v1/subscriptions/{evt.EventType}"
            );

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Failed to fetch subscriptions");
            }

            var json = await response.Content.ReadAsStringAsync();

            var subscribers = JsonSerializer.Deserialize<List<SubscriptionDto>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var results = new List<EventDeliveryResultDto>();

            // 2. Send event to each webhook
            foreach (var sub in subscribers ?? [])
            {
                try
                {
                    var content = new StringContent(
                        JsonSerializer.Serialize(evt),
                        Encoding.UTF8,
                        "application/json"
                    );

                    var webhookResponse = await _httpClient.PostAsync(
                        sub.WebhookUrl,
                        content
                    );

                    results.Add(new EventDeliveryResultDto
                    {
                        WebhookUrl = sub.WebhookUrl,
                        Status = webhookResponse.StatusCode
                    });
                }
                catch (Exception ex)
                {
                    results.Add(new EventDeliveryResultDto
                    {
                        WebhookUrl = sub.WebhookUrl,
                        Error = ex.Message
                    });
                }
            }

            return Ok(new PublishEventResponseDto
            {
                Message = "Event processed",
                SubscribersCount = subscribers?.Count ?? 0,
                DeliveryResults = results
            });
        }
    }
}