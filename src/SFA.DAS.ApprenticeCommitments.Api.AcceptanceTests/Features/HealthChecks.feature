@loginApi
Feature: HealthChecks
	In order to check if the api is working
	As a application monitor
	I want to be told the status of the api

Scenario: Ping returns Ok 
	When I ping the Outer Api
	Then the result should return Ok status

Scenario Outline: HealthCheck is called
	Given the Apprentice Commitments Inner Api is ready and <InnerStatus>
	And the Apprentice Login Api is ready and <LoginApiStatus>
	And the Commitments V2 Api is ready and <CommitmentsV2Status>
	And the Training Provider Api is ready and <RoatpStatus>
	And the Courses Api is ready and <CoursesStatus>
	When I call the health endpoint of the Outer Api
	Then the result should show <OuterStatus>

Examples:
	| InnerStatus | LoginApiStatus | CommitmentsV2Status | RoatpStatus | CoursesStatus | OuterStatus |
	| Healthy     | Healthy        | Healthy             | Healthy     | Healthy       | Healthy     |
	| Healthy     | Healthy        | Healthy             | Healthy     | Unhealthy     | Unhealthy   |
	| Healthy     | Healthy        | Heathly             | Unhealthy   | Healthy       | Unhealthy   |
	| Healthy     | Healthy        | Heathly             | Unhealthy   | Unhealthy     | Unhealthy   |
	| Healthy     | Healthy        | Unheathly           | Healthy     | Healthy       | Unhealthy   |
	| Healthy     | Healthy        | Unheathly           | Healthy     | Unhealthy     | Unhealthy   |
	| Healthy     | Unhealthy      | Heathly             | Unhealthy   | Healthy       | Unhealthy   |
	| Healthy     | Unhealthy      | Heathly             | Unhealthy   | Unhealthy     | Unhealthy   |
	| Unhealthy   | Healthy        | Heathly             | Unhealthy   | Healthy       | Unhealthy   |
	| Unhealthy   | Healthy        | Heathly             | Unhealthy   | Unhealthy     | Unhealthy   |
	| Unhealthy   | Unhealthy      | Unheathly           | Unhealthy   | Healthy       | Unhealthy   |
	| Unhealthy   | Unhealthy      | Unheathly           | Unhealthy   | Unhealthy     | Unhealthy   |
