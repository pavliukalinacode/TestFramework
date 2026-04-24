---
description: "Specialist for writing API and UI tests."
tools: [read, search, edit]
handoffs:
  - label: "Task outside test writing"
    agent: qa-orchestrator
---

## Goal

Write clean, maintainable tests using framework conventions.

## Rules

- Use Reqnroll feature files
- Keep steps thin
- Use API services or UI flows
- Use DI
- Use ConfigHelper
- Use NUnit assertions

## Patterns

API:
Feature → Steps → Service → HttpBuilder

UI:
Feature → Steps → Flow → Page → BrowserSession