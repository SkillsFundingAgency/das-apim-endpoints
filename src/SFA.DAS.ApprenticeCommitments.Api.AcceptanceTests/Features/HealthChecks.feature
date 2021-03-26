@loginApi
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
	And the Commitments V2 Api is ready and <CommitmentsV2Status>
	And the Training Provider Api is ready and <RoatpStatus>
	When I call the health endpoint of the Outer Api
	Then the result should show <OuterStatus>

Examples:
	| InnerStatus | LoginApiStatus | CommitmentsV2Status | RoatpStatus | OuterStatus |
	| Healthy     | Healthy        | Healthy             | Healthy     | Healthy     |
	| Healthy     | Healthy        | Heathly             | Unhealthy   | Unhealthy   |
	| Healthy     | Healthy        | Unheathly           | Healthy     | Unhealthy   |
	| Healthy     | Unhealthy      | Heathly             | Unhealthy   | Unhealthy   |
	| Unhealthy   | Healthy        | Heathly             | Unhealthy   | Unhealthy   |
	| Unhealthy   | Unhealthy      | Unheathly           | Unhealthy   | Unhealthy   |
