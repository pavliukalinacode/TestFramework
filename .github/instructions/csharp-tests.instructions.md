---
description: "Use when writing or modifying C# test code in Api.Tests or UI.Tests."
applyTo: "**/*.cs"
---

# C# Test Instructions

## Framework

- Reqnroll (BDD)
- NUnit (assertions)
- DI via Reqnroll + Microsoft.Extensions.DependencyInjection

## General Rules

- Do NOT edit `.feature.cs` files
- Keep logic OUT of step definitions
- Use DI for services and flows
- Use async/await everywhere
- Do NOT use Thread.Sleep

## API Tests

Flow:

Feature → Steps → Api.Service → HttpBuilder → Response

Rules:

- No HttpClient in steps
- Use Api.Services
- Use Models for payloads
- Use builders from Tests.Data

Example:

```csharp
public sealed class PetSteps(PetService petService)
{
    private ApiResponse<PostPetResponse>? response;

    [When("I create a pet")]
    public async Task WhenICreateAPet()
    {
        var payload = new PostPetPayloadBuilder().Build();
        response = await petService.PostPet<PostPetResponse>(payload);
    }

    [Then("the pet is created")]
    public void ThenThePetIsCreated()
    {
        Assert.That(response, Is.Not.Null);
    }
}