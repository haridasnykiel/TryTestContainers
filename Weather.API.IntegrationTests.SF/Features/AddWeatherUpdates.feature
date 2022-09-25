Feature: Add Weather Updates
  Checks that the Weather api can be used to add weather updates when they come in from the endpoint.
  
  
  Scenario: Add a Weather Update
    Given a weather update has been made with the following details
      | Date       | TemperatureC | Summary |
      | 2022-09-09 | 19           | Warm    |
    When a request for all weather updates have been made
    Then the new weather update is included with:
      | Date       | TemperatureC | Summary |
      | 2022-09-09 | 19           | Warm    |