---
description: "Debug a failing test in the framework."
tools: [read, search]
---

Debug the failing test.

## Steps

1. Identify:
   - Test name
   - Step that fails
   - Exception

2. Check:

### Configuration
- Is ConfigKeys correct?
- Is config section present?

### DI
- Is service registered?
- Is module added?

### API
- Correct endpoint?
- Correct payload?
- Auth issues?

### UI
- Locator changed?
- Page not loaded?
- Auth missing?

3. Suggest fix

## Output

- Root cause
- Exact fix
- File to change