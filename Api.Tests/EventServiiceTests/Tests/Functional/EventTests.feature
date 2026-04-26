Feature: Subscription and Events

Scenario: Publish event to existing subscriber
  Given WireMock requests are reset
  And I have a valid subscription
  And I create the subscription
  Then the response status code should be '200'
  And the created subscription matches the submitted payload

  Given I have an event for the created subscription
  When I publish the event
  Then the response status code should be '200'
  And the event publish message should be "Event processed"
  And the event publish response should show '1' subscriber
  And the webhook should receive the event