@innerApi
@outerApi
Feature: HealthChecks
	In order to check if the api is working
	As a application monitor
	I want to be told the status of the api

Scenario Outline: Ping returns Ok 
	When I ping the Outer Api
	Then the result should return Ok status

Scenario Outline: HealthCheck is called
	Given the Apprentice Commitments Inner Api is ready and <InnerStatus>
	When I call the health endpoint of the Outer Api
	Then the result should show <OuterStatus>

Examples:
	| InnerStatus | OuterStatus |
	| Healthy     | Healthy     |
	| Unhealthy   | Unhealthy   |