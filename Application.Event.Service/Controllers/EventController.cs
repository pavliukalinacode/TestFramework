using Application.Contracts.EventContracts;
using Application.Contracts.SubscriptionContracts;
using Application.Event.Service.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace Application.Event.Service.Controllers
{

    [ApiController]
    [Route("v1/events")]
    public class EventController(ISubscriptionClient subscriptionClient, IWebhookClient webhookClient) : ControllerBase
    {
        private readonly ISubscriptionClient subscriptionClient = subscriptionClient;
        private readonly IWebhookClient webhookClient = webhookClient;

        [HttpPost]
        public async Task<IActionResult> Publish(
            EventDto evt,
            CancellationToken cancellationToken)
        {
            if (evt == null)
                return BadRequest("Event payload is required");

            if (string.IsNullOrWhiteSpace(evt.EventType))
                return BadRequest("EventType is required");

            IReadOnlyList<SubscriptionDto> subscribers;

            try
            {
                subscribers = await subscriptionClient.GetSubscriptionsAsync(
                    evt.EventType,
                    cancellationToken);
            }
            catch
            {
                return StatusCode(500, "Failed to fetch subscriptions");
            }

            var results = new List<EventDeliveryResultDto>();

            foreach (var sub in subscribers)
            {
                try
                {
                    var webhookResponse = await webhookClient.SendAsync(
                        sub.WebhookUrl,
                        evt,
                        cancellationToken);

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
                SubscribersCount = subscribers.Count,
                DeliveryResults = results
            });
        }
    }
}