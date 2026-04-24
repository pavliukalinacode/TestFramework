---
description: "Create a new API or UI test using framework conventions."
tools: [read, search, edit]
---

Create a new test following this framework:

## Steps

1. Identify if this is:
   - API test → use Api.Services
   - UI test → use UI.Apps + UI.Framework

2. Create or update:
   - `.feature` file
   - Step definitions
   - Service / Flow if missing

## Rules

- Do NOT write HTTP calls in steps
- Do NOT use Playwright directly in steps
- Use DI for services/flows
- Use ConfigHelper for config
- Use NUnit assertions

## Output

- Feature file
- Step definitions
- Supporting service/flow (if needed)