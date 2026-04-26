using Api.Services.Models;
using Api.Services.Tools;
using Logging.Logger;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Services.Components
{
    public class WireMockService(HttpClient httpClient, ILog logger)
    {
        private readonly HttpClient httpClient = httpClient;
        private readonly ILog logger = logger;

        public Task<ApiResponse<string>> GetRequests()
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Get)
                .ToEndPoint("/__admin/requests")
                .ExecuteRawAsync();
        }

        public Task<ApiResponse<string>> ResetRequests()
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Delete)
                .ToEndPoint("/__admin/requests")
                .ExecuteRawAsync();
        }
    }
}