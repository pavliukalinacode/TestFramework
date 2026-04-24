# Test Patterns

## API Example

Given I have payload  
When I call API  
Then response is OK  

Flow:
Steps → PetService → HttpBuilder

## UI Example

Given I open login page  
When I login  
Then I see dashboard  

Flow:
Steps → LoginFlow → LoginPage

## Rules

- No HTTP in steps
- No Playwright in steps
- Use DI
- Use async