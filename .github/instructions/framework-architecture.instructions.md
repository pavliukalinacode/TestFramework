---
description: "Use when modifying framework structure, services, DI, UI flows, or API components."
applyTo: "**/*.cs"
---

# Framework Architecture

## Projects

- Api.Services → API logic
- Api.Tests → API tests
- UI.Framework → Playwright core
- UI.Apps → UI logic
- UI.Tests → UI tests
- Configuration → config
- Models → DTOs
- Tests.Data → builders
- Tests.Tools → helpers

## Dependency Injection

- Register dependencies in SetupTestDependencies
- Use constructor injection
- Do NOT manually new services

## API Pattern

Location:
Api.Services/Components

Rules:

- Use HttpClient via DI
- Use HttpBuilder
- Use ConfigHelper
- Return typed responses

## UI Pattern

Split:

- Pages → locators/actions
- Flows → business logic
- Hooks → setup/cleanup

## Playwright

- Use BrowserSession
- No Thread.Sleep
- Use waits properly

## Hooks

- @ui → browser lifecycle
- @auth-standard → login
- @auth-visual → login

## Generated Files

Do NOT edit:

- *.feature.cs
- bin/
- obj/