using Application.Contracts;
using Application.Subscription.Service.Models;
using Application.Subscription.Service.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Application.Subscription.Service.Controllers
{
    [ApiController]
    [Route("subscriptions")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionRepository _repository;

        public SubscriptionsController(ISubscriptionRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(
            SubscriptionDto dto,
            CancellationToken cancellationToken)
        {
            var subscription = new SubscriptionEntity
            {
                EventType = dto.EventType,
                WebhookUrl = dto.WebhookUrl
            };

            await _repository.AddAsync(subscription, cancellationToken);

            return Ok();
        }

        [HttpGet("{eventType}")]
        public async Task<IActionResult> Get(
            string eventType,
            CancellationToken cancellationToken)
        {
            var subscriptions = await _repository.GetByEventTypeAsync(eventType, cancellationToken);
            return Ok(subscriptions);
        }
    }
}