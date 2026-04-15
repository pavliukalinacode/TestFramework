@ui
Feature: Sauce Demo login

  Scenario: Standard user can log in
    When I open the Sauce Demo login page
    And I log in as the standard user
    Then I should be on the inventory page
    And the inventory title should be "Products"

  Scenario: Visual user can log in
    When I open the Sauce Demo login page
    And I log in as the visual user
    Then I should be on the inventory page
    And the inventory title should be "Products"

  @auth-standard
  Scenario: Standard user starts authenticated
    When I open the inventory page
    Then I should be on the inventory page
    And the inventory title should be "Products"

  @auth-visual
  Scenario: Visual user starts authenticated
    When I open the inventory page
    Then I should be on the inventory page
    And the inventory title should be "Products"