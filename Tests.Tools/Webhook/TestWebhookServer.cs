using System;
using System.Collections.Generic;
using System.Linq;
using WireMock.Server;

namespace Tests.Tools.Webhook
{
    public class TestWebhookServer : IDisposable
    {
        private readonly WireMockServer _server;

        public string Url => _server.Url!;

        public TestWebhookServer()
        {
            _server = WireMockServer.Start();
        }

        public IReadOnlyList<string> GetReceivedBodies()
        {
            return _server.LogEntries
                .Select(x => x.RequestMessage!.Body ?? string.Empty)
                .ToList();
        }

        public void Dispose()
        {
            _server.Stop();
            _server.Dispose();
        }
    }
}