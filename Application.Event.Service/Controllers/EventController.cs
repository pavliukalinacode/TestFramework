using Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Application.Event.Service.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventController(HttpClient httpClient, IConfiguration config) : ControllerBase
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _config = config;

        [HttpPost]
        public async Task<IActionResult> Publish(EventDto evt)
        {
            var baseUrl = _config["SubscriptionService__BaseUrl"];

            // 1. Get subscribers
            var response = await _httpClient.GetAsync(
                $"{baseUrl}/subscriptions/{evt.EventType}"
            );

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Failed to fetch subscriptions");
            }

            var json = await response.Content.ReadAsStringAsync();

            var subscribers = JsonSerializer.Deserialize<List<SubscriptionDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // 2. Send event to each webhook
            var results = new List<object>();

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

                    results.Add(new
                    {
                        sub.WebhookUrl,
                        Status = webhookResponse.StatusCode
                    });
                }
                catch (Exception ex)
                {
                    results.Add(new
                    {
                        sub.WebhookUrl,
                        Error = ex.Message
                    });
                }
            }

            return Ok(new
            {
                Message = "Event processed",
                SubscribersCount = subscribers?.Count ?? 0,
                DeliveryResults = results
            });
        }
    }
}