namespace Application.Contracts.EventContracts
{
    public class PublishEventResponseDto
    {
        public string? Message { get; set; }

        public int SubscribersCount { get; set; }

        public List<EventDeliveryResultDto> DeliveryResults { get; set; } = [];
    }
}
