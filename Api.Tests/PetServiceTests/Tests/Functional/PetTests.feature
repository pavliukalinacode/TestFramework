@pet @functional
Feature: Pet API Functional Validation

  In order to ensure the Pet API behaves reliably under valid and invalid conditions
  As an API consumer
  I want requests to be correctly validated, processed, and rejected when necessary

  So that data integrity is preserved and previously identified defects do not reoccur


  @positive
  Scenario: Create and retrieve a pet with special characters in fields
    Given I have a pet with
      | field    | value                    |
      | name     | Mr. Whiskers #1 / Café   |
      | category | Cats & Pets              |
      | tag      | indoor-friendly          |
      | tag      | zażółć-jaźń              |
      | tag      | pet !@#$%^&*()           |
      | status   | available                |
      | photoUrl | https://example.com/cat%20photo.jpg             |
      | photoUrl | https://example.com/cats?name=Fluffy&age=2      |
      | photoUrl | https://example.com/#fluffy                     |
      | photoUrl | https://example.com/image.jpg?size=large        |
      | photoUrl | https://sub.domain.example.com/pet              |
    When I create the pet
    Then the response status code should be '200'
    And the created pet matches the submitted payload
    When I retrieve the pet by id
    Then the response status code should be '200'
    And the retrieved pet matches the submitted payload


  @positive
  Scenario Outline: Create and retrieve a pet with supported status
    Given I have a pet with
      | field    | value     |
      | name     | Fluffy    |
      | category | Cats      |
      | tag      | Friendly  |
      | status   | <status>  |
    When I create the pet
    Then the response status code should be '200'
    When I retrieve the pet by id
    Then the response status code should be '200'
    And the retrieved pet matches the submitted payload

    Examples:
      | status    |
      | available |
      | pending   |
      | sold      |


  # BUG 1: Pet id string type is not validated correctly
  # Expected:
  #   - String id should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 500 Internal Server Error
  #   - Invalid client input causes server-side failure
  @bug @negative @string_pet_id_validation_bug_id @should_fail
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
  @bug @negative @oversized_pet_id_validation_bug_id @should_fail
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
  @bug @negative @string_category_id_validation_bug_id @should_fail
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
  @bug @negative @oversized_category_id_validation_bug_id @should_fail
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
  @bug @negative @string_tag_id_validation_bug_id @should_fail
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
  @bug @negative @oversized_tag_id_validation_bug_id @should_fail
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
  @bug @negative @get_invalid_raw_id_exception_leak_bug_id @should_fail
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
  @bug @negative @delete_invalid_raw_id_exception_leak_bug_id @should_fail
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
  @bug @negative @missing_name_validation_bug_id @should_fail
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
  @bug @negative @missing_category_validation_bug_id @should_fail
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
  @bug @negative @duplicate_id_handling_bug_id @should_fail
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


  # BUG 12: Empty payload is accepted and default entity is created
  # Expected:
  #   - Empty JSON payload should be rejected as invalid input
  #   - HTTP 400 Bad Request
  # Actual:
  #   - API returns HTTP 200 OK
  #   - A pet is created with autogenerated id and empty collections
  #   - Required fields (e.g. name, category) are not validated
  @bug @negative @default_id_for_empty_payload_bug_id @should_fail
  Scenario: Creating a pet with empty json payload should be rejected
    Given I have the pet payload as raw json "{}"
    When I create the pet with invalid data
    Then the response status code should be '200'
    #Then the response status code should be '400' - quarantined until bug is fixed