Feature: HealthCheck

I want the HealthCheck feature to accurately report the status of its dependencies

Scenario Outline: Api reports its status
	Given the Earnings Inner Api is <EarningsInner>
	And the Apprenticeships Inner Api is <ApprenticeshipsInner>
	And the Collection Calendar Api is <CollectionCalendarInner>
	When I request the service status
	Then the result should be <OuterStatus>

Examples:
	| EarningsInner    | ApprenticeshipsInner | CollectionCalendarInner | OuterStatus |
	| Ok               | Ok                   | Ok                      | Healthy     |
	| InvalidOperation | Ok                   | Ok                      | Unhealthy   |
	| Ok               | InvalidOperation     | Ok                      | Unhealthy   |
	| Ok               | Ok                   | InvalidOperation        | Unhealthy   |
