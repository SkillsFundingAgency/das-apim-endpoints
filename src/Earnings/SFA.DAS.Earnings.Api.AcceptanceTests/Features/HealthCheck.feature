Feature: HealthCheck

I want the HealthCheck feature to accurately report the status of its dependencies

Scenario: Api reports status as Healthy
	Given the Earnings Inner Api is Ok
	And the Apprenticeships Inner Api is Ok
	And the Collection Calendar Api is Ok
	When I request the service status
	Then the result should be OK

