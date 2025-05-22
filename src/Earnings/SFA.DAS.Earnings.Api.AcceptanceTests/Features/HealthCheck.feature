Feature: HealthCheck

I want the HealthCheck feature to reflect the status of the service dependencies

Scenario Outline: HealthCheck endpoint reports status of service
	Given the Earnings Inner Api status is <EarningsInner>
	And the Apprenticeships Inner Api status is <ApprenticeshipsInner>
	And the Collection Calendar Api status is <CollectionCalendarInner>
	When I request the service status
	Then the result should be <ExpectedStatus>

Examples:
	| EarningsInner       | ApprenticeshipsInner | CollectionCalendarInner | ExpectedStatus |
	| Ok                  | Ok                   | Ok                      | Healthy        |
	| InternalServerError | Ok                   | Ok                      | Unhealthy      |
	| Ok                  | InternalServerError  | Ok                      | Unhealthy      |
	| Ok                  | Ok                   | InternalServerError     | Unhealthy      |
