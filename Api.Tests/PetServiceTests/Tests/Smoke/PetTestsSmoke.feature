@pet @smoke
Feature: Create pet

  Scenario: Create pet with available status
    Given I have a pet with status "available"
    When I create the pet
    Then the create response should be successful
    And the created pet should have status "available"
    And the created pet should match the submitted payload
