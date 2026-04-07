using Api.Services.Models;
using Api.Services.Tools;
using Configuration.Config;
using Models.PetService.Payload;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Tools.Logger;

namespace Api.Services.Components
{
    public class PetService
    {
        private readonly HttpClient httpClient;
        private readonly ILog logger;
        private readonly string version;
        private readonly int postPetTimeout;

        public PetService(HttpClient httpClient, ILog logger, ConfigHelper configHelper)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ArgumentNullException.ThrowIfNull(configHelper);

            version = configHelper.GetRequiredString(ConfigKeys.ScenariosSection, ConfigKeys.PetTests, ConfigKeys.Version);
            postPetTimeout = configHelper.GetRequiredInt(ConfigKeys.ScenariosSection, ConfigKeys.PetTests, ConfigKeys.Timeout, ConfigKeys.PostPet);
        }

        public Task<ApiResponse<T>> PostPet<T>(PostPetPayload payload)
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Post)
                .ToEndPoint($"/{version}/pet")
                .AddBody(payload)
                .WithTimeout(postPetTimeout)
                .ExecuteAsync<T>();
        }

        public Task<ApiResponse<string>> PostRawPetBody<T>(string rawJson)
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Post)
                .ToEndPoint($"/{version}/pet")
                .AddRawJsonBody(rawJson)
                .WithTimeout(postPetTimeout)
                .ExecuteRawAsync();
        }

        public Task<ApiResponse<T>> GetPetById<T>(string petId)
        {
            return new HttpBuilder(httpClient, logger)
                 .Method(HttpMethod.Get)
                 .ToEndPoint($"/{version}/pet/{petId}")
                 .ExecuteAsync<T>();
        }

        public Task<ApiResponse<T>> DeletePetById<T>(string petId)
        {
            return new HttpBuilder(httpClient, logger)
                .Method(HttpMethod.Delete)
                .ToEndPoint($"/{version}/pet/{petId}")
                .ExecuteAsync<T>();
        }
    }
}