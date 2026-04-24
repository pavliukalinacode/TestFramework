# TestFramework Copilot Instructions

## Project Overview

This repository is a .NET test automation framework for API and UI testing.

Main areas:

- `Api.Services` — API service clients and HTTP helpers.
- `Api.Tests` — API BDD tests using Reqnroll + NUnit.
- `UI.Framework` — Playwright browser/session framework.
- `UI.Apps` — UI application page objects, flows, and auth helpers.
- `UI.Tests` — UI BDD tests using Reqnroll + NUnit.
- `Configuration` — appsettings loading and strongly named config keys.
- `Models` — request/response models.
- `Tests.Data` — test payload builders and data helpers.
- `Tests.Tools` — logging and shared utilities.

## Tech Stack

- C#
- .NET 10
- NUnit
- Reqnroll
- Reqnroll.Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Configuration
- HttpClient
- Playwright

## General Rules

- Follow existing folder structure and naming patterns.
- Prefer extending existing services, modules, page objects, flows, and step classes.
- Do not hardcode URLs, credentials, browser settings, or timeouts.
- Read values from `Configuration/appsettings.*.json` through `ConfigHelper` and `ConfigKeys`.
- Register dependencies through DI modules or `SetupTestDependencies`.
- Keep test steps readable and scenario-focused.
- Keep HTTP logic inside service classes, not directly in step definitions.
- Keep Playwright locator and page interaction logic inside page objects or flows, not directly in step definitions.
- Use async/await consistently.
- Do not use `Thread.Sleep`; use proper retry, Playwright waits, or framework helpers.
- Do not edit generated `.feature.cs` files directly. Edit `.feature` files and step definitions instead.

## API Testing Pattern

API clients live in `Api.Services/Components`.

Register API clients in modules under:

```text
Api.Services/Modules