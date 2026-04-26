using Application.Contracts.EventContracts;
using System;

namespace Tests.Data.EventService.PayloadBuilder
{
    public class EventPayloadBuilder
    {
        private string eventType = $"UserCreated-{Guid.NewGuid():N}";

        private string payload = """
        {
          "id": "qa-user-1",
          "name": "Test User"
        }
        """;

        public EventPayloadBuilder SetEventType(string value)
        {
            eventType = value;
            return this;
        }

        public EventPayloadBuilder SetPayload(string value)
        {
            payload = value;
            return this;
        }

        public EventDto Build()
        {
            return new EventDto
            {
                EventType = eventType,
                Payload = payload
            };
        }
    }
}