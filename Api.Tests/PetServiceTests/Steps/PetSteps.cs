using Api.Services.Components;
using API.Services.Models;
using API.Tests.Base;
using Configuration.Config;
using Microsoft.Extensions.DependencyInjection;
using Models.PetService.General;
using Models.PetService.Payload;
using Models.PetService.Response;
using Reqnroll;
using System.Threading.Tasks;
using Tests.Data.PetService.PayloadBuilder;
using Tests.Tools.Logger;

namespace API.Tests.PetServiceTests.Steps
{
    [Binding]
    public sealed class PetSteps
    {
        private readonly PetService petService;
        private readonly ILog logger;
        private readonly ConfigHelper configHelper;
        private readonly ScenarioContext scenarioContext;

        public PetSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;

            var provider = Hooks.Provider;

            petService = provider.GetRequiredService<PetService>();
            logger = provider.GetRequiredService<ILog>();
            configHelper = provider.GetRequiredService<ConfigHelper>();
        }


        [Given("I have a pet with status {string}")]
        public void GivenIHaveAPetPayloadWithStatus(string status)
        {
            var payload = new PostPetPayloadBuilder()
                .SetStatus(status)
                .Build();

            logger.Info($"Pet id {payload.Id}");

            scenarioContext[nameof(PostPetPayload)] = payload;
        }

        [When("I create the pet")]
        public async Task WhenICreateThePet()
        {
            var request = (PostPetPayload)scenarioContext[nameof(PostPetPayload)];
            var response = await petService.PostPet<PostPetResponse>(request);

            scenarioContext[nameof(ApiResponse<PostPetResponse>)] = response;
        }

        [Then("the create response should be successful")]
        public void ThenTheCreateResponseShouldBeSuccessful()
        {
            var response = (ApiResponse<PostPetResponse>)scenarioContext[nameof(ApiResponse<PostPetResponse>)];

            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccessStatusCode, Is.True);
                Assert.That(response.Data, Is.Not.Null);
            });
        }

        [Then("the created pet should have status {string}")]
        public void ThenTheCreatedPetShouldHaveStatus(string expectedStatus)
        {
            var response = (ApiResponse<PostPetResponse>)scenarioContext[nameof(ApiResponse<PostPetResponse>)];

            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Status, Is.EqualTo(expectedStatus));
        }

        [Then("the created pet should match the submitted payload")]
        public void ThenTheCreatedPetShouldMatchTheSubmittedPayload()
        {
            var payload = (PostPetPayload)scenarioContext[nameof(PostPetPayload)];
            var response = (ApiResponse<PostPetResponse>)scenarioContext[nameof(ApiResponse<PostPetResponse>)];

            Assert.That(response.Data, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(response.Data!.Id, Is.EqualTo(payload.Id));
                Assert.That(response.Data.Name, Is.EqualTo(payload.Name));
                Assert.That(response.Data.Status, Is.EqualTo(payload.Status));
            });
        }
    }
}