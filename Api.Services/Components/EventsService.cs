using Api.Services.Models;
using Api.Services.Tools;
using Application.Contracts.EventContracts;
using Configuration.Config;
using Logging.Logger;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Services.Components
{
    public class EventsService
    {
        private readonly HttpClient httpClient;
        private readonly ILog logger;
        private readonly string version;

        public EventsService(HttpClient httpClient, ILog logger, ConfigHelper configHelper)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ArgumentNullException.ThrowIfNull(configHelper);

            version = configHelper.GetRequired<string>(
                ConfigKeys.ScenariosSection,
                ConfigKeys.EventsTests,
                ConfigKeys.Version);
        }

        public Task<ApiResponse<T>> PublishEvent<T>(EventDto payload)
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Post)
                .ToEndPoint($"/{version}/events")
                .AddBody(payload)
                .ExecuteAsync<T>();
        }
    }
}