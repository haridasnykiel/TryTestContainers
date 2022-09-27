Feature: GetWeatherUpdates
	Get weather updates

Scenario: Get A Weather Update 
	Given a weather update has been made with the following details
	  | Date       | TemperatureC | Summary |
	  | 2022-09-09 | 19           | Warm    |
	When a request for a weather update with:
	  | Date       | TemperatureC |
	  | 2022-09-09 | 19           |
	Then the new weather update is included with:
	  | Date       | TemperatureC | Summary |
	  | 2022-09-09 | 19           | Warm    |