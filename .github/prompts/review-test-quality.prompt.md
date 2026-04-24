---
description: "Review test quality and framework compliance."
tools: [read, search]
---

Review this test.

## Check:

### Structure
- Uses feature + steps
- No logic in steps

### API
- Uses service layer
- No raw HttpClient

### UI
- Uses flows/pages
- No direct Playwright in steps

### Config
- Uses ConfigHelper
- No hardcoded values

### Async
- Uses async/await
- No Thread.Sleep

### Assertions
- NUnit
- Clear expectations

## Output

- Issues found
- Suggested improvements