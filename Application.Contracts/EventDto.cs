namespace Application.Contracts
{
    public class EventDto
    {
        public string EventType { get; set; } = default!;
        public string Payload { get; set; } = default!;
    }
}
