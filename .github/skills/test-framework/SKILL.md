---
name: test-framework
description: "Understand the architecture and patterns of the test framework."
---

## Architecture

Testing → Services/Flows → Framework → Config

## API Pattern

Steps → Api.Service → HttpBuilder → Response

## UI Pattern

Steps → Flow → Page → BrowserSession

## Key Principles

- DI everywhere
- No logic in steps
- No hardcoding config
- Async only