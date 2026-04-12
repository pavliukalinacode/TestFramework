@pet @smoke
Feature: Pet Api Smoke Tests
  In order to verify the core pet workflow
  As an API consumer
  I want to create, retrieve, and delete a pet successfully

  Scenario: Create retrieve and delete a pet successfully
    Given I have a pet with
      | field    | value                 |
      | name     | Fluffy                |
      | category | Cats                  |
      | tag      | Friendly              |
      | tag      | Domesticated          |
      | photoUrl | https://example.com/a |
      | photoUrl | https://example.com/b |
      | status   | available             |
    When I create the pet
    Then the response status code should be '200'
    And the created pet matches the submitted payload

    When I retrieve the pet by id
    Then the response status code should be '200'
    And the retrieved pet matches the submitted payload

    When I delete the pet by id
    Then the response status code should be '200'