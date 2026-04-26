using Api.Services.Models;
using Api.Services.Tools;
using Application.Contracts.SubscriptionContracts;
using Configuration.Config;
using Logging.Logger;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Services.Components
{
    public class SubscriptionService
    {
        private readonly HttpClient httpClient;
        private readonly ILog logger;
        private readonly string version;

        public SubscriptionService(HttpClient httpClient, ILog logger, ConfigHelper configHelper)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ArgumentNullException.ThrowIfNull(configHelper);

            version = configHelper.GetRequired<string>(
                ConfigKeys.ScenariosSection,
                ConfigKeys.SubscriptionTests,
                ConfigKeys.Version);
        }

        public Task<ApiResponse<T>> CreateSubscription<T>(SubscriptionDto payload)
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Post)
                .ToEndPoint($"/{version}/subscriptions")
                .AddBody(payload)
                .ExecuteAsync<T>();
        }

        public Task<ApiResponse<T>> GetSubscriptionsByEventType<T>(string eventType)
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Get)
                .ToEndPoint($"/{version}/subscriptions/{eventType}")
                .ExecuteAsync<T>();
        }
    }
}