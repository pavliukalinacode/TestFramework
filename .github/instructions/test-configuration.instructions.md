---
description: "Use when modifying configuration, appsettings, or ConfigKeys."
applyTo: "**/appsettings*.json,**/ConfigKeys.cs"
---

# Configuration

## Loading

Controlled by:

CLOUD environment variable

| CLOUD | File |
|------|------|
| not set | appsettings.Local.json |
| Development | appsettings.Development.json |

## Rules

- Do NOT hardcode values
- Use ConfigHelper
- Centralize keys in ConfigKeys

## Sections

- GlobalParameters
- Scenarios
- Playwright
- Logger

## Example Usage

```csharp
var baseUrl = configHelper.GetRequired<string>(
    ConfigKeys.ScenariosSection,
    ConfigKeys.PetTests,
    ConfigKeys.BaseUrl);