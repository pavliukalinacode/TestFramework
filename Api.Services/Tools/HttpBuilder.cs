using Api.Services.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tests.Tools.Logger;

namespace Api.Services.Tools
{
    /// <summary>
    /// Fluent builder for constructing HTTP requests.
    /// Supports setting method, endpoint, headers, body, query parameters, and timeout.
    /// Handles execution and response deserialization.
    /// </summary>
    public sealed class HttpBuilder
    {
        private Func<HttpRequestMessage, HttpRequestMessage> configFunction;
        private readonly HttpClient client;
        private readonly ILog logger;
        private TimeSpan? timeout;
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(30);
        private const int MaxLoggedContentLength = 3000;

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
            var timeoutValue = timeout > 0
                ? TimeSpan.FromSeconds(timeout)
                : DefaultTimeout;

            return WithTimeout(timeoutValue);
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
                request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
                return request;
            });

            return this;
        }

        public HttpBuilder AddRawJsonBody(string rawJson)
        {
            configFunction = Compose(configFunction, request =>
            {
                request.Content = new StringContent(rawJson, Encoding.UTF8, MediaTypeNames.Application.Json);
                return request;
            });

            return this;
        }

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

                    logger.Debug(
                        $"Response deserialized successfully to type '{typeof(T).Name}'.");
                }
                catch (JsonException ex)
                {
                    logger.Error(ex,
                        $"Failed to deserialize response to type '{typeof(T).Name}'. Response content: {TrimForLog(rawResponse.Content)}");
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

        public Task<ApiResponse<string>> ExecuteRawAsync()
        {
            return ExecuteCoreAsync();
        }

        private async Task<ApiResponse<string>> ExecuteCoreAsync()
        {
            using var request = Build();
            using var cts = new CancellationTokenSource(timeout ?? DefaultTimeout);

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string? content = null;
            bool isSuccessStatusCode = false;
            string? errorMessage = null;

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await LogRequestAsync(request);

                using var response = await client.SendAsync(request, cts.Token);

                stopwatch.Stop();

                statusCode = response.StatusCode;
                isSuccessStatusCode = response.IsSuccessStatusCode;
                content = await response.Content.ReadAsStringAsync();

                LogResponse(response, content, stopwatch.Elapsed);
            }
            catch (TaskCanceledException ex)
            {
                stopwatch.Stop();

                errorMessage = timeout.HasValue
                    ? $"HTTP request timed out after {timeout.Value.TotalSeconds} seconds."
                    : $"HTTP request was canceled. {ex.Message}";

                logger.Error(ex, errorMessage);
                statusCode = HttpStatusCode.RequestTimeout;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                errorMessage = $"HTTP exception while sending request to '{request.RequestUri}': {ex.Message}";
                logger.Error(ex, errorMessage);
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

        private async Task LogRequestAsync(HttpRequestMessage request)
        {
            logger.Info("Sending HTTP request...");
            logger.Debug($"Request Method: {request.Method}");
            logger.Debug($"Request URI: {request.RequestUri}");
            logger.Debug($"Timeout: {(timeout ?? DefaultTimeout).TotalSeconds} seconds");

            if (request.Headers.Any())
            {
                var headers = string.Join("; ",
                    request.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"));

                logger.Debug($"Request Headers: {headers}");
            }

            if (request.Content != null)
            {
                if (request.Content.Headers.Any())
                {
                    var contentHeaders = string.Join("; ",
                        request.Content.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"));

                    logger.Debug($"Request Content Headers: {contentHeaders}");
                }

                var body = await request.Content.ReadAsStringAsync();
                logger.Debug($"Request Body: {TrimForLog(body)}");
            }
        }

        private void LogResponse(HttpResponseMessage response, string? content, TimeSpan elapsed)
        {
            logger.Info(
                $"Received HTTP response. StatusCode: {(int)response.StatusCode} ({response.StatusCode}), Duration: {elapsed.TotalMilliseconds:F0} ms");

            if (response.Headers.Any())
            {
                var headers = string.Join("; ",
                    response.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"));

                logger.Debug($"Response Headers: {headers}");
            }

            if (response.Content?.Headers != null && response.Content.Headers.Any())
            {
                var contentHeaders = string.Join("; ",
                    response.Content.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"));

                logger.Debug($"Response Content Headers: {contentHeaders}");
            }

            if (!string.IsNullOrWhiteSpace(content))
            {
                logger.Debug($"Response Body: {TrimForLog(content)}");
            }
        }

        private static string TrimForLog(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return value.Length <= MaxLoggedContentLength
                ? value
                : value[..MaxLoggedContentLength] + "... [truncated]";
        }
    }
}