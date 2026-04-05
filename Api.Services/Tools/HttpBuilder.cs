using API.Services.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tests.Tools.Logger;

namespace API.Services.Tools
{
    public sealed class HttpBuilder
    {
        private Func<HttpRequestMessage, HttpRequestMessage> configFunction;
        private readonly HttpClient client;
        private readonly ILog logger;
        private TimeSpan? timeout;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);

        public HttpBuilder(HttpClient client, ILog logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            configFunction = x => x;
        }

        private static Func<A, C> Compose<A, B, C>(Func<A, B> f1, Func<B, C> f2)
        {
            return a => f2(f1(a));
        }

        public HttpBuilder WithTimeout(TimeSpan? timeout)
        {
            this.timeout = (timeout.HasValue && timeout.Value > TimeSpan.Zero)
                ? timeout.Value
                : DefaultTimeout;

            return this;
        }

        public HttpBuilder WithTimeout(int timeout)
        {
            var timeoutSeconds = timeout > 0
                ? TimeSpan.FromSeconds(timeout)
                : DefaultTimeout;

            return WithTimeout(timeoutSeconds);
        }

        private async Task<ApiResponse<string>> ExecuteCoreAsync()
        {
            using var request = Build();
            using var cts = new CancellationTokenSource(timeout ?? DefaultTimeout);

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string? content = null;
            bool isSuccessStatusCode = false;
            string? errorMessage = null;

            try
            {
                logger.Debug($"Request URI: {request.RequestUri}");
                logger.Debug($"Request Method: {request.Method}");

                using var response = cts is not null
                    ? await client.SendAsync(request, cts.Token)
                    : await client.SendAsync(request);

                statusCode = response.StatusCode;
                isSuccessStatusCode = response.IsSuccessStatusCode;
                content = await response.Content.ReadAsStringAsync();

                logger.Info($"Response StatusCode: {response.StatusCode}");
            }
            catch (TaskCanceledException ex)
            {
                errorMessage = timeout.HasValue
                    ? $"HTTP request timed out after {timeout.Value.TotalSeconds} seconds."
                    : $"HTTP request was canceled. {ex.Message}";

                logger.Error(errorMessage);
                statusCode = HttpStatusCode.RequestTimeout;
            }
            catch (Exception ex)
            {
                errorMessage = $"HTTP exception: {ex.Message}";
                logger.Error(errorMessage);
            }

            return new ApiResponse<string>
            {
                StatusCode = statusCode,
                Data = content,
                Content = content,
                IsSuccessStatusCode = isSuccessStatusCode,
                ErrorMessage = errorMessage
            };
        }

        public HttpRequestMessage Build()
        {
            return configFunction(new HttpRequestMessage());
        }

        public HttpBuilder Method(HttpMethod method)
        {
            configFunction = Compose(configFunction, request =>
            {
                request.Method = method;
                return request;
            });

            return this;
        }

        public HttpBuilder ToEndPoint(string resource)
        {
            configFunction = Compose(configFunction, request =>
            {
                request.RequestUri = new Uri(resource, UriKind.Relative);
                return request;
            });

            return this;
        }

        public HttpBuilder AddBody(object contractObject)
        {
            configFunction = Compose(configFunction, request =>
            {
                var json = JsonConvert.SerializeObject(contractObject);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                return request;
            });

            return this;
        }

        //demonstrating adding parameters despite not using directly in current scenarios to show flexibility of the builder for future use cases
        public HttpBuilder AddParameter(string param, string value)
        {
            configFunction = Compose(configFunction, request =>
            {
                var uri = request.RequestUri?.ToString() ?? string.Empty;
                var separator = uri.Contains('?') ? '&' : '?';

                uri += $"{separator}{param}={Uri.EscapeDataString(value)}";
                request.RequestUri = new Uri(uri, UriKind.Relative);

                return request;
            });

            return this;
        }

        //demonstrating adding headers despite not using directly in current scenarios to show flexibility of the builder for future use cases
        public HttpBuilder AddHeader(string name, string value)
        {
            configFunction = Compose(configFunction, request =>
            {
                request.Headers.TryAddWithoutValidation(name, value);
                return request;
            });

            return this;
        }

        public async Task<ApiResponse<T>> ExecuteAsync<T>()
        {
            var rawResponse = await ExecuteCoreAsync();

            T? data = default;

            if (!string.IsNullOrWhiteSpace(rawResponse.Content))
            {
                try
                {
                    data = JsonConvert.DeserializeObject<T>(rawResponse.Content);
                }
                catch (JsonException ex)
                {
                    logger.Error($"Deserialization exception: {ex.Message}");
                }
            }

            return new ApiResponse<T>
            {
                StatusCode = rawResponse.StatusCode,
                Data = data,
                Content = rawResponse.Content,
                IsSuccessStatusCode = rawResponse.IsSuccessStatusCode,
                ErrorMessage = rawResponse.ErrorMessage
            };
        }
    }
}