# API Test Framework

## Overview

This repository contains a modular API automation framework built with:

- .NET
- NUnit
- Reqnroll
- HttpClient
- configuration-driven environment setup

## Dependencies
Api.Tests
 ├── Api.Services
 ├── Models
 ├── Configuration
 ├── Tests.Data
 └── Tests.Tools

Api.Services
 ├── Models
 ├── Configuration
 └── Tests.Tools

Tests.Data
 └── Models

Configuration
 └── (no project dependencies)

Models
 └── (no project dependencies)

## Postman

A simple **Postman collection and environment file** are included to support manual API exploration and debugging.

- The Postman collection mirrors key API operations (create, retrieve, delete pet)
- The environment file contains base URL and configurable variables

This allows:
- quick validation outside automation
- easier debugging of failing scenarios
- collaboration with non-automation stakeholders

## Configuration

Configuration is managed via `appsettings` and environment-based overrides.

## GitHub Actions Integration

- Sensitive and environment-specific values (e.g. base URL) are stored as **GitHub Variables**
- During pipeline execution, these values are injected into the test runtime
- Pipeline Execution happens using **Github Actions** and is built in, but without **writing access** you wont be able to trigget the pipeline. Pipeline file is stored in .github/workflows/functional-tests.yml
- The framework automatically resolves configuration based on environment
<img width="1614" height="1167" alt="image" src="https://github.com/user-attachments/assets/72c89b34-94f2-478f-bdba-815d5361b11e" />


BDD Reporting
BDD test reports are generated automatically during execution

Output location:
/bin/<configuration>/TestResults
A sample report is included in:
/Reports
<img width="868" height="1314" alt="image" src="https://github.com/user-attachments/assets/38825e4d-bc2c-4daa-be8f-5315992e1c7b" />

## Test Coverage

The framework currently includes:

**1 Smoke test**
**16 Functional tests**

Positive scenarios: validate expected behavior (create, retrieve, valid inputs)
Negative scenarios: validate invalid inputs and error handling

Important Note
Negative tests are intentionally implemented as **bug scenarios**

Reason:
The API under test (Swagger Petstore) is a demo service
It contains multiple inconsistencies and validation gaps
These behaviors are documented and treated as defects
Approach:
Bugs are explicitly described in scenario comments
Tests are written to reflect actual current behavior
Expected correct behavior is documented but quarantined

## Logging

The framework provides two logging modes depending on the execution environment.

### CI Execution (Safe Logging)

For CI/CD runs, a **safe logger** is used.
It prevents exposure of sensitive or excessive data

### Configuration

The logger type is controlled via `appsettings`.

- Default configuration in CI uses the **safe logger**
- This is enforced via GitHub Variables
- Local runs can override this to use the console logger

### Example (Local Debug Output)

Below is an example of verbose console logging during local execution:
<img width="2213" height="1229" alt="image" src="https://github.com/user-attachments/assets/d50859d6-d57b-45ab-89da-854dab2d9bd4" />

## Notes on Task Completion

### AI Usage
AI tools were used only for consultation and guidance.
No direct access to the codebase was provided, and all implementation decisions and code were written independently.

### Time Spent
The task exceeded the expected 2-hour timeframe.
The goal was not just to complete the task, but to use it as an opportunity for personal growth and framework enhancement.

### Scope & Review Guidance
The solution includes more functionality and structure than strictly required for the task.

To save your time:
- please feel free to review only the parts that are most relevant or interesting
- the framework is modular, so individual components can be assessed independently

Thank you for your time and consideration!
