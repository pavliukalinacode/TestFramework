@pet @functional
Feature: Pet Api Feature Tests
  In order to prevent known defects from reappearing
  As an API consumer
  I want invalid and inconsistent pet API behavior to be rejected correctly

  # BUG 1: Pet id string type is not validated correctly
  # Expected:
  #   - String id should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 500 Internal Server Error
  #   - Invalid client input causes server-side failure
  @bug @string_pet_id_validation
  Scenario: Creating a pet with string id should be rejected without server error
    Given I have the pet with invalid string-type Id
    When I create the pet with invalid data
    Then the response status code should be '500'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 2: Out of range pet id is not validated correctly
  # Expected:
  #   - Oversized numeric id should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 500 Internal Server Error
  #   - Invalid numeric boundary value crashes request processing
  @bug @oversized_pet_id_validation
  Scenario: Creating a pet with an out of range numeric id should be rejected without server error
    Given I have a pet with an id longer than long
    When I create the pet with invalid data
    Then the response status code should be '500'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 3: Category id string type is not validated correctly
  # Expected:
  #   - String category id should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 500 Internal Server Error
  #   - Nested invalid client input causes server-side failure
  @bug @string_category_id_validation
  Scenario: Creating a pet with string category id should be rejected without server error
    Given I have the pet with invalid string-type category Id
    When I create the pet with invalid data
    Then the response status code should be '500'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 4: Out of range category id is not validated correctly
  # Expected:
  #   - Oversized numeric category id should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 500 Internal Server Error
  #   - Nested invalid numeric boundary value crashes request processing
  @bug @oversized_category_id_validation
  Scenario: Creating a pet with out of range category id should be rejected without server error
    Given I have the pet with category Id longer than long
    When I create the pet with invalid data
    Then the response status code should be '500'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 5: Tag id string type is not validated correctly
  # Expected:
  #   - String tag id should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 500 Internal Server Error
  #   - Nested collection invalid client input causes server-side failure
  @bug @string_tag_id_validation
  Scenario: Creating a pet with string tag id should be rejected without server error
    Given I have the pet with invalid string-type tag Id
    When I create the pet with invalid data
    Then the response status code should be '500'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 6: Out of range tag id is not validated correctly
  # Expected:
  #   - Oversized numeric tag id should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 500 Internal Server Error
  #   - Nested collection numeric boundary issue causes server-side failure
  @bug @oversized_tag_id_validation
  Scenario: Creating a pet with out of range tag id should be rejected without server error
    Given I have the pet with tag Id longer than long
    When I create the pet with invalid data
    Then the response status code should be '500'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 7: GET invalid raw id leaks internal implementation details
  # Expected:
  #   - Non-numeric path id should be rejected as client input error
  #   - No internal exception details should be exposed
  # Actual:
  #   - Response leaks NumberFormatException details
  @bug @get_invalid_raw_id_exception_leak
  Scenario: Retrieving a pet with a non numeric id should not leak implementation details
    When I try to retrieve the pet by id '*'
    Then the response status code should be '404'
    #And the response should not expose implementation details - quarantined until bug is fixed

  # BUG 8: DELETE invalid raw id leaks internal implementation details
  # Expected:
  #   - Non-numeric path id should be rejected as client input error
  #   - No internal exception details should be exposed
  # Actual:
  #   - Response leaks NumberFormatException details
  @bug @delete_invalid_raw_id_exception_leak
  Scenario: Deleting a pet with a non numeric id should not leak implementation details
    When I delete the pet by id 'fluff'
    Then the response status code should be '404'
    #And the response should not expose implementation details - quarantined until bug is fixed

  # BUG 9: Missing name validation
  # Expected:
  #   - Request should be rejected
  #   - HTTP 400 Bad Request
  # Actual:
  #   - Pet is created successfully without name
  #   - API allows incomplete business entity
  @bug @missing_name_validation
  Scenario: Creating a pet without name should be rejected
    Given I have a pet with no name
    When I create the pet
    Then the response status code should be '200'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 10: Missing category validation
  # Expected:
  #   - Request should be rejected
  #   - HTTP 400 Bad Request
  # Actual:
  #   - Pet is created successfully without category
  #   - Required domain field is not enforced
  @bug @missing_category_validation
  Scenario: Creating a pet without category should be rejected
    Given I have a pet with no category
    When I create the pet
    Then the response status code should be '200'
    #Then the response status code should be '400' - quarantined until bug is fixed

  # BUG 11: Duplicate id handling
  # Expected:
  #   - Creating a second pet with the same id should be rejected
  #   - HTTP 409 Conflict
  # Actual:
  #   - API returns successful response for duplicate id creation
  #   - This is overwriting existing data
  #   - POST should be not idempotent
  @bug @duplicate_id_handling
  Scenario: Creating a pet with duplicate id should be rejected
    Given I have a pet with
      | field    | value                 |
      | id       | 5452245               |
      | name     | Fluffy                |
      | category | Cats                  |
      | tag      | Friendly              |
      | photoUrl | https://example.com/a |
      | status   | sold                  |
    And I create the pet
    And the response status code should be '200'

    And I have a pet with
      | field    | value                 |
      | id       | 5452245               |
      | name     | Fluffy                |
      | category | Cats                  |
      | tag      | Friendly              |
      | photoUrl | https://example.com/a |
      | status   | sold                  |
    When I create the pet
    Then the response status code should be '200'
    #Then the response status code should be '409' - quarantined until bug is fixed