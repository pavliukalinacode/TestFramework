using Api.Services.Components;
using Api.Services.Models;
using Models.PetService.Payload;
using Models.PetService.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Reqnroll;
using System.Numerics;
using System.Threading.Tasks;
using Tests.Data.PetService.PayloadBuilder;
using Tests.Tools.Logger;

namespace Api.Tests.PetServiceTests.Steps
{
    [Binding]
    public sealed class PetSteps(ScenarioContext scenarioContext, PetService petService)
    {
        private const string InvalidPet = nameof(InvalidPet);

        private const string BigNumber = "111111111111111111111111111111111111111111111111";

        [Given(@"I have a pet with")]
        public void GivenIHaveAPetWith(Table table)
        {
            var builder = new PostPetPayloadBuilder();

            foreach (var row in table.Rows)
            {
                builder.Apply(row["field"], row["value"]);
            }
            var p = builder.Build();
            scenarioContext.Set(p);
        }

        [Given("I have a pet with no name")]
        public void GivenIHaveAPetWithNoName()
        {
            scenarioContext.Set(
                new PostPetPayloadBuilder()
                    .SetName(null)
                    .Build());
        }

        [Given("I have a pet with no category")]
        public void GivenIHaveAPetWithNoCategory()
        {
            scenarioContext.Set(
                new PostPetPayloadBuilder()
                    .SetCategory(null)
                    .Build());
        }

        [Given(@"I have the pet payload as raw json ""(.*)""")]
        public void GivenIHaveThePetPayloadAsRawJson(string json)
        {
            scenarioContext.Set(json);
        }

        [Given(@"I have the pet with invalid string-type Id")]
        public void GivenIHaveThePetWithInvalidStringTypeId()
        {
            SaveInvalidPetRequest(root => root["id"] = InvalidPet);
        }

        [Given(@"I have a pet with an id longer than long")]
        public void GivenIHaveAPetWithAnIdLongerThanLong()
        {
            SaveInvalidPetRequest(root =>
                root["id"] = JToken.FromObject(BigInteger.Parse(BigNumber)));
        }

        [Given(@"I have the pet with invalid string-type category Id")]
        public void GivenIHaveThePetWithInvalidStringTypeCategoryId()
        {
            SaveInvalidPetRequest(root =>
                ((JObject)root["category"]!)["id"] = InvalidPet);
        }

        [Given(@"I have the pet with category Id longer than long")]
        public void GivenIHaveThePetWithCategoryIdLongerThanLong()
        {
            SaveInvalidPetRequest(root =>
                ((JObject)root["category"]!)["id"] =
                    JToken.FromObject(BigInteger.Parse(BigNumber)));
        }

        [Given(@"I have the pet with invalid string-type tag Id")]
        public void GivenIHaveThePetWithInvalidStringTypeTagId()
        {
            SaveInvalidPetRequest(root =>
                ((JObject)((JArray)root["tags"]!)[0]!)["id"] = InvalidPet);
        }

        [Given(@"I have the pet with tag Id longer than long")]
        public void GivenIHaveThePetWithTagIdLongerThanLong()
        {
            SaveInvalidPetRequest(root =>
                ((JObject)((JArray)root["tags"]!)[0]!)["id"] =
                    JToken.FromObject(BigInteger.Parse(BigNumber)));
        }

        [Given("I create the pet")]
        [When("I create the pet")]
        public async Task WhenICreateThePet()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = await petService.PostPet<PostPetResponse>(payload);

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I create the pet with invalid data")]
        public async Task WhenICreateThePetWithInvalidData()
        {
            var body = scenarioContext.Get<string>();
            var response = await petService.PostRawPetBody<string>(body);

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I retrieve the pet by id")]
        public async Task WhenIRetrieveThePetById()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = await petService.GetPetById<GetPetResponse>(payload.Id.ToString());

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I retrieve the pet by id few times")]
        public async Task WhenIRetrieveThePetByIdFewTimes()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = await petService.GetPetByIdWithRetry<GetPetResponse>(payload.Id.ToString());

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I try to retrieve the pet by id")]
        public async Task WhenITryToRetrieveThePetById()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = await petService.GetPetById<ErrorPetResponse>(payload.Id.ToString());

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I try to retrieve the pet by id '(.*)'")]
        public async Task WhenITryToRetrieveThePetById(string petId)
        {
            var response = await petService.GetPetById<ErrorPetResponse>(petId);

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I delete the pet by id")]
        public async Task WhenIDeleteThePetById()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = await petService.DeletePetById<ErrorPetResponse>(payload.Id.ToString());

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I delete the pet by id few times")]
        public async Task WhenIDeleteThePetByIdFewTimes()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = await petService.DeletePetByIdWithRetry<ErrorPetResponse>(payload.Id.ToString());

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [When(@"I delete the pet by id '(.*)'")]
        public async Task WhenIDeleteThePetById(string petId)
        {
            var response = await petService.DeletePetById<ErrorPetResponse>(petId);

            scenarioContext.Set(response);
            scenarioContext.Set((int)response.StatusCode);
        }

        [Given(@"the response status code should be '(.*)'")]
        [Then(@"the response status code should be '(.*)'")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            var actualStatusCode = scenarioContext.Get<int>();

            Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode));
        }

        [Then(@"the created pet matches the submitted payload")]
        public void ThenTheCreatedPetMatchesTheSubmittedPayload()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = scenarioContext.Get<ApiResponse<PostPetResponse>>();

            AssertPetMatchesPayload(response.Data!, payload);
        }

        [Then(@"the retrieved pet matches the submitted payload")]
        public void ThenTheRetrievedPetMatchesTheSubmittedPayload()
        {
            var payload = scenarioContext.Get<PostPetPayload>();
            var response = scenarioContext.Get<ApiResponse<GetPetResponse>>();

            AssertPetMatchesPayload(response.Data!, payload);
        }

        private void SaveInvalidPetRequest(System.Action<JObject> mutate)
        {
            var payload = new PostPetPayloadBuilder().Build();
            var rawPayload = JObject.FromObject(payload);

            mutate(rawPayload);

            scenarioContext.Set(rawPayload.ToString(Formatting.None));
        }

        private static void AssertPetMatchesPayload(dynamic actual, dynamic expected)
        {
            Assert.Multiple(() =>
            {
                Assert.That((long)actual.Id, Is.EqualTo(expected.Id));
                Assert.That((string?)actual.Name, Is.EqualTo(expected.Name));
                Assert.That((string?)actual.Status, Is.EqualTo(expected.Status));

                Assert.That(actual.Category, Is.Not.Null, "Expected category in response.");
                Assert.That(expected.Category, Is.Not.Null, "Expected category in payload.");
                Assert.That((long)actual.Category.Id, Is.EqualTo(expected.Category!.Id));
                Assert.That((string?)actual.Category.Name, Is.EqualTo(expected.Category.Name));

                Assert.That(actual.PhotoUrls, Is.EqualTo(expected.PhotoUrls), "PhotoUrls mismatch.");

                Assert.That(actual.Tags, Is.Not.Null, "Expected tags in response.");
                Assert.That(expected.Tags, Is.Not.Null, "Expected tags in payload.");
                Assert.That((int)actual.Tags.Count, Is.EqualTo(expected.Tags!.Count), "Tag count mismatch.");

                for (var i = 0; i < expected.Tags.Count; i++)
                {
                    Assert.That((long)actual.Tags[i].Id, Is.EqualTo(expected.Tags[i].Id), $"Tag[{i}].Id mismatch.");
                    Assert.That((string?)actual.Tags[i].Name, Is.EqualTo(expected.Tags[i].Name), $"Tag[{i}].Name mismatch.");
                }
            });
        }
    }
}