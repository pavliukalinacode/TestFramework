using Api.Services.Components;
using Api.Tests.Base;
using API.Services.Models;
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
        private readonly ScenarioContext scenarioContext;
        private readonly PetService petService;
        private readonly ILog logger;
        private readonly TestExecutionContext executionContext;

        public PetSteps(ScenarioContext scenarioContext, PetService petService, ILog logger, TestExecutionContext executionContext)
        {
            this.scenarioContext = scenarioContext;
            this.petService = petService;
            this.logger = logger;
            this.executionContext = executionContext;
        }

        [Given("I have a pet with status {string}")]
        public void GivenIHaveAPetPayloadWithStatus(string status)
        {
            var payload = new PostPetPayloadBuilder()
                .SetStatus(status)
                .Build();

            logger.Info($"Scenario id: {executionContext.ScenarioId}");
            logger.Info($"Pet id {payload.Id}");

            scenarioContext.Set(payload);
        }

        [When("I create the pet")]
        public async Task WhenICreateThePet()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = await petService.PostPet<PostPetResponse>(payload);

            scenarioContext.Set(response);
        }

        [Then("the create response should be successful")]
        public void ThenTheCreateResponseShouldBeSuccessful()
        {
            var response = scenarioContext.Get<ApiResponse<PostPetResponse>>();

            Assert.Multiple(() =>
            {
                Assert.That(response.IsSuccessStatusCode, Is.True);
                Assert.That(response.Data, Is.Not.Null);
            });
        }

        [Then("the created pet should have status {string}")]
        public void ThenTheCreatedPetShouldHaveStatus(string expectedStatus)
        {
            var response = scenarioContext.Get<ApiResponse<PostPetResponse>>();

            Assert.That(response.Data!.Status, Is.EqualTo(expectedStatus));
        }

        [Then("the created pet should match the submitted payload")]
        public void ThenTheCreatedPetShouldMatchTheSubmittedPayload()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = scenarioContext.Get<ApiResponse<PostPetResponse>>();

            Assert.Multiple(() =>
            {
                Assert.That(response.Data!.Id, Is.EqualTo(payload.Id));
                Assert.That(response.Data.Name, Is.EqualTo(payload.Name));
                Assert.That(response.Data.Status, Is.EqualTo(payload.Status));
            });
        }
    }
}