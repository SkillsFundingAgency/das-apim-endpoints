@loginApi
@innerApi
Feature: HealthChecks
	In order to check if the api is working
	As a application monitor
	I want to be told the status of the api

Scenario Outline: Ping returns Ok 
	When I ping the Outer Api
	Then the result should return Ok status

Scenario Outline: HealthCheck is called
	Given the Apprentice Commitments Inner Api is ready and <InnerStatus>
	And the Apprentice Login Api is ready and <LoginApiStatus>
	When I call the health endpoint of the Outer Api
	Then the result should show <OuterStatus>

Examples:
	| InnerStatus | LoginApiStatus | OuterStatus |
	| Healthy     | Healthy        | Healthy     |
	| Healthy     | Unhealthy      | Unhealthy   |
	| Unhealthy   | Healthy        | Unhealthy   |
	| Unhealthy   | Unhealthy      | Unhealthy   |